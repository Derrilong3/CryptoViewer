using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Handlers.CoinCap.WebResponseClasses;
using CryptoViewer.Handlers.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoViewer.Handlers.CoinCap
{
    [Export(typeof(IApiHandler))]
    internal class CoinCapHandler : PropertyChangedBase, IApiHandler
    {
        private const string GetExchangersUrl = "https://api.coincap.io/v2/exchanges";
        private const string GetPairsByExchangersUrl = "https://api.coincap.io/v2/markets";
        private const string GetCoinsDatatUrl = "https://api.coincap.io/v2/assets";

        private IRestClient _restClient;

        private IEnumerable<ICoin> _coinGeckos;
        private IEnumerable<ICoin> _coinsCap;
        private Dictionary<string, Dictionary<string, double[][]>> _candles;

        [ImportingConstructor]
        public CoinCapHandler(IRestClient client)
        {
            _candles = new Dictionary<string, Dictionary<string, double[][]>>();
            _restClient = client;
        }

        public async Task<IEnumerable<IExchanger>> GetExchangers()
        {
            try
            {
                Root<Exchanger[]> root = await _restClient.GetAsync<Root<Exchanger[]>>(GetExchangersUrl);

                return root.Data;
            }
            catch (Exception ex)
            {
                // future message box
                Debug.WriteLine(ex);
                return Enumerable.Empty<IExchanger>();
            }
        }

        public async Task<IEnumerable<IPair>> GetExchangers(ICoin coin)
        {
            try
            {
                string url = $"{GetCoinsDatatUrl}/{coin.Id}/markets";
                Root<PairByCurrency[]> root = await _restClient.GetAsync<Root<PairByCurrency[]>>(url);

                return root.Data;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<IPair>();
            }
        }

        public async Task<IEnumerable<ICoin>> GetCurrencies()
        {
            try
            {
                if (_coinsCap != null)
                    return _coinsCap;

                NameValueCollection data = new NameValueCollection
                {
                    { "limit", "2000" }
                };

                Root<Coin[]> root = await _restClient.GetAsync<Root<Coin[]>>(GetCoinsDatatUrl, data);

                _coinsCap = root.Data;

                return _coinsCap;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ICoin>();
            }
        }

        public async Task<ICoin> GetCurrency(string id)
        {
            try
            {
                string url = $"{GetCoinsDatatUrl}/{id}";
                Root<Coin> coin = await _restClient.GetAsync<Root<Coin>>(url);

                return coin.Data;
            }
            catch (Exception ex)
            {
                return new Coin();
            }
        }

        public async Task<IEnumerable<IPair>> GetCurrencies(IExchanger exchanger)
        {
            try
            {
                NameValueCollection data = new NameValueCollection
                {
                    { "exchangeId", exchanger.Id },
                    { "limit", 30.ToString() }
                };

                Root<InnerPair[]> root = await _restClient.GetAsync<Root<InnerPair[]>>(GetPairsByExchangersUrl, data);

                return root.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Enumerable.Empty<InnerPair>();
            }
        }

        public async Task<double[][]> GetOHLC(ICoin coin, string interval)
        {
            try
            {
                if (_coinGeckos == null)
                    _coinGeckos = await GetCoinGecko();

                string id = _coinGeckos.First(x => x.Symbol.ToLower() == coin.Symbol.ToLower()).Id;

                if (_candles.TryGetValue(id, out var intervals) && intervals.ContainsKey(interval))
                {
                    return intervals[interval];
                }

                string getOHCLUrl = $"https://api.coingecko.com/api/v3/coins/{id}/ohlc";

                NameValueCollection data = new NameValueCollection
                {
                    { "vs_currency", "usd" },
                    { "days", interval }
                };

                double[][] result = await _restClient.GetAsync<double[][]>(getOHCLUrl, data);

                if (_candles.TryGetValue(id, out intervals))
                {
                    intervals.Add(interval, result);
                }
                else
                {
                    intervals = new Dictionary<string, double[][]>()
                    {
                        { interval, result }
                    };

                    _candles.Add(id, intervals);
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new double[0][];
            }
        }

        private async Task<IEnumerable<ICoin>> GetCoinGecko()
        {
            string getCoinList = "https://api.coingecko.com/api/v3/coins/list";

            Coin[] coins = await _restClient.GetAsync<Coin[]>(getCoinList);

            return coins;
        }
    }
}

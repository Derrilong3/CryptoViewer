using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Handlers.CoinCap.WebResponseClasses;
using CryptoViewer.Handlers.Models;
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
        private const string GetCoinsDataUrl = "https://api.coincap.io/v2/assets";

        private readonly IRestClient _restClient;
        private readonly Dictionary<string, Dictionary<string, double[][]>> _candles;

        private IEnumerable<ICoin> _coinGeckos;
        private IEnumerable<ICoin> _coinsCap;

        [ImportingConstructor]
        public CoinCapHandler(IRestClient client)
        {
            _candles = new Dictionary<string, Dictionary<string, double[][]>>();
            _restClient = client;
        }

        public async Task<IEnumerable<IExchanger>> GetExchangersAsync()
        {
            try
            {
                var root = await _restClient.GetAsync<Root<Exchanger[]>>(GetExchangersUrl);

                return root.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Enumerable.Empty<IExchanger>();
            }
        }

        public async Task<IEnumerable<IPair>> GetExchangersAsync(ICoin coin)
        {
            try
            {
                var url = $"{GetCoinsDataUrl}/{coin.Id}/markets";
                var root = await _restClient.GetAsync<Root<PairByCurrency[]>>(url);

                return root.Data;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<IPair>();
            }
        }

        public async Task<IEnumerable<ICoin>> GetCurrenciesAsync()
        {
            try
            {
                if (_coinsCap != null)
                    return _coinsCap;

                var data = new NameValueCollection
                {
                    { "limit", "2000" }
                };

                var root = await _restClient.GetAsync<Root<Coin[]>>(GetCoinsDataUrl, data);

                _coinsCap = root.Data;

                return _coinsCap;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ICoin>();
            }
        }

        public async Task<ICoin> GetCurrencyAsync(string id)
        {
            try
            {
                var url = $"{GetCoinsDataUrl}/{id}";
                var coin = await _restClient.GetAsync<Root<Coin>>(url);

                return coin.Data;
            }
            catch (Exception ex)
            {
                return new Coin();
            }
        }

        public async Task<IEnumerable<IPair>> GetCurrenciesAsync(IExchanger exchanger)
        {
            try
            {
                var data = new NameValueCollection
                {
                    { "exchangeId", exchanger.Id },
                    { "limit", 30.ToString() }
                };

                var root = await _restClient.GetAsync<Root<InnerPair[]>>(GetPairsByExchangersUrl, data);

                return root.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Enumerable.Empty<InnerPair>();
            }
        }

        public async Task<double[][]> GetOhlcAsync(ICoin coin, string interval)
        {
            try
            {
                _coinGeckos ??= await GetCoinGecko();

                var id = _coinGeckos.First(x => string.Equals(x.Symbol, coin.Symbol, StringComparison.CurrentCultureIgnoreCase)).Id;

                if (_candles.TryGetValue(id, out var intervals) && intervals.TryGetValue(interval, out var ohlc))
                {
                    return ohlc;
                }

                var getOhclUrl = $"https://api.coingecko.com/api/v3/coins/{id}/ohlc";

                var data = new NameValueCollection
                {
                    { "vs_currency", "usd" },
                    { "days", interval }
                };

                var result = await _restClient.GetAsync<double[][]>(getOhclUrl, data);

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
                return Array.Empty<double[]>();
            }
        }

        private async Task<IEnumerable<ICoin>> GetCoinGecko()
        {
            string getCoinList = "https://api.coingecko.com/api/v3/coins/list";

            var coins = await _restClient.GetAsync<Coin[]>(getCoinList);

            return coins;
        }
    }
}

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

namespace CryptoViewer.Handlers.CoinCap
{
    [Export(typeof(IApiHandler))]
    internal class CoinCapHandler : PropertyChangedBase, IApiHandler
    {
        private const string GetExchangersUrl = "https://api.coincap.io/v2/exchanges";
        private const string GetPairsByExchangersUrl = "https://api.coincap.io/v2/markets";
        private const string GetCoinsDatatUrl = "https://api.coincap.io/v2/assets";

        private IWebFetcher _webFetcher;

        private IEnumerable<ICoin> _coinGeckos;
        private Dictionary<string, Dictionary<string, double[][]>> _candles;

        [ImportingConstructor]
        public CoinCapHandler(IWebFetcher webFetcher)
        {
            _candles = new Dictionary<string, Dictionary<string, double[][]>>();
            _webFetcher = webFetcher;
        }

        public IEnumerable<IExchanger> GetExchangers()
        {
            try
            {
                string rawJson = _webFetcher.Fetch(GetExchangersUrl, "GET");

                Root<Exchanger> root = JsonConvert.DeserializeObject<Root<Exchanger>>(rawJson);

                return root.Data;
            }
            catch (Exception ex)
            {
                // future message box
                Debug.WriteLine(ex);
                return Enumerable.Empty<IExchanger>();
            }
        }

        public IEnumerable<IPair> GetExchangers(ICoin coin)
        {
            try
            {
                string url = $"{GetCoinsDatatUrl}/{coin.Id}/markets";
                string rawJson = _webFetcher.Fetch(url, "GET");

                Root<PairByCurrency> root = JsonConvert.DeserializeObject<Root<PairByCurrency>>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return root.Data;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<IPair>();
            }
        }

        public IEnumerable<ICoin> GetCurrencies()
        {
            try
            {
                NameValueCollection data = new NameValueCollection
                {
                    { "limit", "2000" }
                };

                string rawJson = _webFetcher.Fetch(GetCoinsDatatUrl, "GET", data);

                Root<Coin> root = JsonConvert.DeserializeObject<Root<Coin>>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return root.Data;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ICoin>();
            }
        }

        public IEnumerable<IPair> GetCurrencies(IExchanger exchanger)
        {
            try
            {
                NameValueCollection data = new NameValueCollection
                {
                    { "exchangeId", exchanger.Id },
                    { "limit", 30.ToString() }
                };

                string rawJson = _webFetcher.Fetch(GetPairsByExchangersUrl, "GET", data);

                Root<InnerPair> root = JsonConvert.DeserializeObject<Root<InnerPair>>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return root.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Enumerable.Empty<InnerPair>();
            }
        }

        public double[][] GetOHLC(ICoin coin, string interval)
        {
            try
            {
                if (_coinGeckos == null)
                    _coinGeckos = GetCoinGecko();

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

                string rawJson = _webFetcher.Fetch(getOHCLUrl, "GET", data);

                double[][] result = JsonConvert.DeserializeObject<double[][]>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

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

        private IEnumerable<ICoin> GetCoinGecko()
        {
            string getCoinList = "https://api.coingecko.com/api/v3/coins/list";

            string rawJson = _webFetcher.Fetch(getCoinList, "GET");

            Coin[] coins = JsonConvert.DeserializeObject<Coin[]>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return coins;
        }
    }
}

using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Handlers.CoinCap.WebResponseClasses;
using CryptoViewer.Handlers.Models;
using CryptoViewer.Utilities;
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
        private const string GetPairsByMarketUrl = "https://api.coincap.io/v2/markets";
        private const string GetCoinsDatatUrl = "https://api.coincap.io/v2/assets";

        private IEnumerable<ICoin> _coinGeckos;

        public CoinCapHandler()
        {
            string s = WebFetcher.Fetch("https://api.coincap.io/v2/candles?exchange=binance&interval=h8&baseId=BTC&quoteId=USD", "GET");
        }

        public IEnumerable<IExchanger> GetExchangers()
        {
            try
            {
                string rawJson = WebFetcher.Fetch(GetExchangersUrl, "GET");

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

        public IEnumerable<ICoin> GetExchangers(ICoin exchanger)
        {
            try
            {
                return Enumerable.Empty<ICoin>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ICoin>();
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

                string rawJson = WebFetcher.Fetch(GetCoinsDatatUrl, "GET", data);

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

                string rawJson = WebFetcher.Fetch(GetPairsByMarketUrl, "GET", data);

                Root<InnerPair> root = JsonConvert.DeserializeObject<Root<InnerPair>>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return root.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Enumerable.Empty<InnerPair>();
            }
        }

        public double[][] GetOHLC(ICoin coin, int interval)
        {
            try
            {
                string rawJson;

                if (_coinGeckos == null)
                {
                    string getCoinList = "https://api.coingecko.com/api/v3/coins/list";

                    rawJson = WebFetcher.Fetch(getCoinList, "GET");

                    _coinGeckos = JsonConvert.DeserializeObject<Coin[]>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                string id = _coinGeckos.First(x => x.Symbol.ToLower() == coin.Symbol.ToLower()).Id;

                string getOHCLUrl = $"https://api.coingecko.com/api/v3/coins/{id}/ohlc";

                NameValueCollection data = new NameValueCollection
                {
                    { "vs_currency", "usd" },
                    { "days", interval.ToString() }
                };

                rawJson = WebFetcher.Fetch(getOHCLUrl, "GET", data);

                double[][] result = JsonConvert.DeserializeObject<double[][]>(rawJson, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new double[0][];
            }
        }
    }
}

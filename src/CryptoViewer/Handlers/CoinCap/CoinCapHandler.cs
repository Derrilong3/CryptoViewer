using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Handlers.CoinCap.WebResponseClasses;
using CryptoViewer.Handlers.Models;
using Newtonsoft.Json;
using SkinsTradeAssistant.Models;
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
        private const string GetPriceByMarketUrl = "https://api.coincap.io/v2/markets";

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

        public IEnumerable<ICurrency> GetExchangers(ICurrency exchanger)
        {
            try
            {
                return Enumerable.Empty<ICurrency>();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ICurrency>();
            }
        }

        public IEnumerable<ICurrency> GetCurrencies()
        {
            try
            {
                return Enumerable.Empty<ICurrency>();

            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ICurrency>();
            }
        }

        public IEnumerable<InnerPair> GetCurrencies(IExchanger exchanger)
        {
            try
            {
                NameValueCollection data = new NameValueCollection
                {
                    { "exchangeId", exchanger.Id },
                    { "limit", 10.ToString() }
                };

                string rawJson = WebFetcher.Fetch(GetPriceByMarketUrl, "GET", data);

                Root<InnerPair> root = JsonConvert.DeserializeObject<Root<InnerPair>>(rawJson);

                return root.Data;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Enumerable.Empty<InnerPair>();
            }
        }
    }
}

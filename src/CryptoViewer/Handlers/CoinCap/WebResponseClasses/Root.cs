using CryptoViewer.Handlers.Models;
using Newtonsoft.Json;
using System;

namespace CryptoViewer.Handlers.CoinCap.WebResponseClasses
{
    internal class Root<T>
    {
        [JsonProperty("data")]
        public T[] Data { get; set; }
    }
}

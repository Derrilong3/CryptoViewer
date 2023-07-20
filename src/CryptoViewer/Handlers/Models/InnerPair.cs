using CryptoViewer.Utilities.Attributes;
using Newtonsoft.Json;

namespace CryptoViewer.Handlers.Models
{
    internal class InnerPair : Pair
    {
        [ItemColumnData("Volume (%)")]
        [JsonProperty("percentExchangeVolume")]
        public string VolumePercent { get; set; }

        [ItemColumnData("Trades (24h)")]
        [JsonProperty("tradesCount24Hr")]
        public int TradesCount { get; set; }
    }
}

using Newtonsoft.Json;

namespace CryptoViewer.Handlers.Models
{
    internal class InnerPair : Pair
    {
        [JsonProperty("percentExchangeVolume")]
        public string VolumePercent { get; set; }

        [JsonProperty("tradesCount24Hr")]
        public string TradesCount { get; set; }
    }
}

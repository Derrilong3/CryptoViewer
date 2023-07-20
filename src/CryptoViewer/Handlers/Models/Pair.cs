using CryptoViewer.Base.Interfaces;
using Newtonsoft.Json;
namespace CryptoViewer.Handlers.Models
{
    internal abstract class Pair : IPair
    {
        public string PairName => $"{BaseSymbol}/{QuoteSymbol}";

        [JsonProperty("baseId")]
        public string BaseId { get; set; }

        [JsonProperty("baseSymbol")]
        public string BaseSymbol { get; set; }

        [JsonProperty("exchangeId")]
        public string ExhangeId { get; set; }

        [JsonProperty("priceUsd")]
        public float PriceUsd { get; set; }

        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        [JsonProperty("quoteSymbol")]
        public string QuoteSymbol { get; set; }

        [JsonProperty("volumeUsd24Hr")]
        public float VolumeUsd { get; set; }
    }
}

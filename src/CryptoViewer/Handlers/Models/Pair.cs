using CryptoViewer.Base.Interfaces;
using CryptoViewer.Utilities.Attributes;
using Newtonsoft.Json;
namespace CryptoViewer.Handlers.Models
{
    internal abstract class Pair : IPair
    {
        [ItemColumnData("Pair")]
        public string PairName => $"{BaseSymbol}/{QuoteSymbol}";

        [JsonProperty("baseId")]
        public string BaseId { get; set; }

        [JsonProperty("baseSymbol")]
        public string BaseSymbol { get; set; }

        [JsonProperty("exchangeId")]
        public virtual string ExhangeId { get; set; }

        [ItemColumnData("Price")]
        [JsonProperty("priceUsd")]
        public float PriceUsd { get; set; }

        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        [JsonProperty("quoteSymbol")]
        public string QuoteSymbol { get; set; }

        [ItemColumnData("Volume (24h)", 100, "{0:N0}")]
        [JsonProperty("volumeUsd24Hr")]
        public float VolumeUsd { get; set; }
    }
}

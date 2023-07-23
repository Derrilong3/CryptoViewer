using CryptoViewer.Utilities.Attributes;
using Newtonsoft.Json;

namespace CryptoViewer.Handlers.Models
{
    internal class PairByCurrency : Pair
    {
        [ItemColumnData("Exchange", 150)]
        [JsonProperty("exchangeId")]
        public override string ExhangeId { get; set; }
    }
}

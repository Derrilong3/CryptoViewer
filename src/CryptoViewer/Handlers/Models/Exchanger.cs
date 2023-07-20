using CryptoViewer.Base.Interfaces;
using Newtonsoft.Json;

namespace CryptoViewer.Handlers.Models
{
    internal class Exchanger : IExchanger
    {
        [JsonProperty("exchangeId")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("volumeUsd")]
        public float? VolumeUsd { get; set; }
    }
}

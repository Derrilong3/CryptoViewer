using Newtonsoft.Json;

namespace CryptoViewer.Handlers.CoinCap.WebResponseClasses
{
    internal class Root<T>
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}

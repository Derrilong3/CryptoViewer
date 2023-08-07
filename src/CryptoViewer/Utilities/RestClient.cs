using CryptoViewer.Base.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CryptoViewer.Utilities
{
    [Export(typeof(IRestClient))]
    public class RestClient : IRestClient
    {
        private static HttpClient _httpClient;
        private JsonSerializerSettings _jsonSettings;

        public RestClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36");
            _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

            _jsonSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        private async Task<string> SendRequestAsync(string url, HttpMethod httpMethod, HttpContent content = null, NameValueCollection headers = null)
        {
            using (var request = new HttpRequestMessage(httpMethod, url))
            {
                if (headers != null)
                {
                    for (int i = 0; i < headers.Count; i++)
                    {
                        request.Headers.Add(headers.Keys[i], headers[i]);
                    }
                }

                if (content != null)
                {
                    request.Content = content;
                }

                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<T> GetAsync<T>(string url, NameValueCollection query = null, NameValueCollection headers = null)
        {
            string response = await GetAsync(url, query, headers);

            return JsonConvert.DeserializeObject<T>(response, _jsonSettings);
        }

        public Task<string> GetAsync(string url, NameValueCollection query = null, NameValueCollection headers = null)
        {
            if (query != null)
            {
                string dataString = String.Join("&", Array.ConvertAll(query.AllKeys, key =>
                string.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(query[key]))
                ));

                url += (url.Contains("?") ? "&" : "?") + dataString;
            }

            return SendRequestAsync(url, HttpMethod.Get, null, headers);
        }

        public async Task<T> PostAsync<T>(string url, NameValueCollection data = null, NameValueCollection headers = null)
        {
            string response = await PostAsync(url, data, headers);

            return JsonConvert.DeserializeObject<T>(response, _jsonSettings);
        }

        public Task<string> PostAsync(string url, NameValueCollection data = null, NameValueCollection headers = null)
        {
            var content = new StringContent(data == null ? string.Empty : String.Join("&", Array.ConvertAll(data.AllKeys, key =>
                string.Format("{0}={1}", WebUtility.UrlEncode(key), WebUtility.UrlEncode(data[key]))
            )), Encoding.UTF8, "application/x-www-form-urlencoded");

            return SendRequestAsync(url, HttpMethod.Post, content, headers);
        }

        public async Task<R> PostAsJsonAsync<T, R>(string url, T data, NameValueCollection headers = null)
        {
            string response = await PostAsJsonAsync(url, data, headers);

            return JsonConvert.DeserializeObject<R>(response, _jsonSettings);
        }

        public Task<string> PostAsJsonAsync<T>(string url, T data, NameValueCollection headers = null)
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            return SendRequestAsync(url, HttpMethod.Post, content, headers);
        }
    }
}

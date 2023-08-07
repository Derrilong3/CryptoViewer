using System.Collections.Specialized;
using System.Threading.Tasks;

namespace CryptoViewer.Base.Services
{
    internal interface IRestClient
    {
        Task<T> GetAsync<T>(string url, NameValueCollection query = null, NameValueCollection headers = null);
        Task<string> GetAsync(string url, NameValueCollection query = null, NameValueCollection headers = null);
        Task<T> PostAsync<T>(string url, NameValueCollection data = null, NameValueCollection headers = null);
        Task<string> PostAsync(string url, NameValueCollection data = null, NameValueCollection headers = null);
        Task<R> PostAsJsonAsync<T, R>(string url, T data, NameValueCollection headers = null);
        Task<string> PostAsJsonAsync<T>(string url, T data, NameValueCollection headers = null);
    }
}

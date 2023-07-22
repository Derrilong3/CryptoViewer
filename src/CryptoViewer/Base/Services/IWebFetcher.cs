using System.Collections.Specialized;
using System.Net;

namespace CryptoViewer.Base.Services
{
    internal interface IWebFetcher
    {
        CookieContainer Cookies { get; }

        string Fetch(string url, string method, NameValueCollection data = null, bool ajax = true, string referer = "", bool fetchError = false, NameValueCollection headers = null);
        HttpWebResponse Request(string url, string method, NameValueCollection data = null, bool ajax = true, string referer = "", bool fetchError = false, NameValueCollection headers = null);
    }
}

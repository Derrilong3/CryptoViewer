using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoViewer.Base.Services
{
    internal interface IApiHandler
    {
        Task<IEnumerable<IExchanger>> GetExchangersAsync();
        Task<IEnumerable<IPair>> GetExchangersAsync(ICoin coin);
        Task<IEnumerable<ICoin>> GetCurrenciesAsync();
        Task<ICoin> GetCurrencyAsync(string id);
        Task<IEnumerable<IPair>> GetCurrenciesAsync(IExchanger exchanger);
        Task<double[][]> GetOhlcAsync(ICoin coin, string interval);
    }
}

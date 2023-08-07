using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoViewer.Base.Services
{
    internal interface IApiHandler
    {
        Task<IEnumerable<IExchanger>> GetExchangers();
        Task<IEnumerable<IPair>> GetExchangers(ICoin coin);
        Task<IEnumerable<ICoin>> GetCurrencies();
        Task<ICoin> GetCurrency(string id);
        Task<IEnumerable<IPair>> GetCurrencies(IExchanger exchanger);
        Task<double[][]> GetOHLC(ICoin coin, string interval);
    }
}

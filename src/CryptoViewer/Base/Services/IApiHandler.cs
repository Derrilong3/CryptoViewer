using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;

namespace CryptoViewer.Base.Services
{
    internal interface IApiHandler
    {
        IEnumerable<IExchanger> GetExchangers();
        IEnumerable<ICoin> GetExchangers(ICoin exchanger);
        IEnumerable<ICoin> GetCurrencies();
        IEnumerable<IPair> GetCurrencies(IExchanger exchanger);
        double[][] GetOHLC(ICoin coin, string interval);
    }
}

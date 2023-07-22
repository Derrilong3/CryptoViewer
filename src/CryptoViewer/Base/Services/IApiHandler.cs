using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;

namespace CryptoViewer.Base.Services
{
    internal interface IApiHandler
    {
        IEnumerable<IExchanger> GetExchangers();
        IEnumerable<IPair> GetExchangers(ICoin coin);
        IEnumerable<ICoin> GetCurrencies();
        IEnumerable<IPair> GetCurrencies(IExchanger exchanger);
        double[][] GetOHLC(ICoin coin, string interval);
    }
}

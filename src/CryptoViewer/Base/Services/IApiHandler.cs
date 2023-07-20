using CryptoViewer.Base.Interfaces;
using CryptoViewer.Handlers.Models;
using System.Collections.Generic;

namespace CryptoViewer.Base.Services
{
    internal interface IApiHandler
    {
        IEnumerable<IExchanger> GetExchangers();
        IEnumerable<ICurrency> GetExchangers(ICurrency exchanger);
        IEnumerable<ICurrency> GetCurrencies();
        IEnumerable<IPair> GetCurrencies(IExchanger exchanger);
    }
}

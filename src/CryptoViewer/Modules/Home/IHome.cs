using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;

namespace CryptoViewer.Modules.Home
{
    internal interface IHome
    {
        IEnumerable<IExchanger> Exchangers { get; }
        IEnumerable<IPair> Pairs { get; }
    }
}

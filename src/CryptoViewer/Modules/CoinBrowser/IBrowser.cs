using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;

namespace CryptoViewer.Modules.CoinBrowser
{
    internal interface IBrowser
    {
        string SearchFieldText { get; }
        IEnumerable<ICoin> Currencies { get; }
    }
}

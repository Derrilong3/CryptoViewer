using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;

namespace CryptoViewer.Modules.CurrencyBrowser
{
    internal interface IBrowser
    {
        string SearchFieldText { get; }
        IEnumerable<ICoin> Currencies { get; }
    }
}

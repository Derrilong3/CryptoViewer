using CryptoViewer.Base.Interfaces;
using CryptoViewer.Utilities.GridViewUtilities;
using System.Collections.Generic;

namespace CryptoViewer.Modules.CoinBrowser
{
    internal interface IBrowser
    {
        string SearchFieldText { get; }
        GridViewHandler GridHandler { get; }

        IEnumerable<ICoin> Currencies { get; }
    }
}

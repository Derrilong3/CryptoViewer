using CryptoViewer.Base.Interfaces;
using System.Collections.Generic;

namespace CryptoViewer.Modules.Converter
{
    internal interface IConverter
    {
        IEnumerable<ICoin> Currencies { get; }
        ICoin FirstCoin { get; }
        ICoin SecondCoin { get; }
    }
}

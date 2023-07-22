using Caliburn.Micro;
using CryptoViewer.Modules.MainMenu.Models;

namespace CryptoViewer.Modules.Converter
{
    internal class ConverterMenuItem : IMenuItem
    {
        public string Name => "Converter";

        public IScreen UserInterface => (IScreen)IoC.Get<IConverter>();
    }
}

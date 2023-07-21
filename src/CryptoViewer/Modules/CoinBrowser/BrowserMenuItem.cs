using Caliburn.Micro;
using CryptoViewer.Modules.Home;
using CryptoViewer.Modules.MainMenu.Models;

namespace CryptoViewer.Modules.CoinBrowser
{
    internal class BrowserMenuItem : IMenuItem
    {
        public string Name => "Search";

        public IScreen UserInterface => (IScreen)IoC.Get<IBrowser>();
    }
}

using Caliburn.Micro;
using CryptoViewer.Modules.MainMenu.Models;

namespace CryptoViewer.Modules.Home
{
    internal class HomeMenuItem : IMenuItem
    {
        public string Name => "Home";

        public IScreen UserInterface => (IScreen)IoC.Get<IHome>();
    }
}

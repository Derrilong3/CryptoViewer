using Caliburn.Micro;
using CryptoViewer.Base;
using System.ComponentModel.Composition;

namespace CryptoViewer.Modules.CoinBrowser
{
    [Export(typeof(IModule))]
    internal class Module : IModule
    {
        public string Name => "Search";

        public IScreen UserInterface => (IScreen)IoC.Get<IBrowser>();
    }
}

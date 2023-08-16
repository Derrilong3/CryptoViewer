using Caliburn.Micro;
using CryptoViewer.Base;
using System.ComponentModel.Composition;

namespace CryptoViewer.Modules.Home
{
    [Export(typeof(IModule))]
    internal class Module : IModule
    {
        public string Name => "Home";

        public IScreen UserInterface => (IScreen)IoC.Get<IHome>();
    }
}

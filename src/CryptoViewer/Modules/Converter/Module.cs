using Caliburn.Micro;
using CryptoViewer.Base;
using System.ComponentModel.Composition;

namespace CryptoViewer.Modules.Converter
{
    [Export(typeof(IModule))]
    internal class Module : IModule
    {
        public string Name => "Converter";

        public IScreen UserInterface => (IScreen)IoC.Get<IConverter>();
    }
}

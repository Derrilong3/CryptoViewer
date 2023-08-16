using Caliburn.Micro;

namespace CryptoViewer.Base
{
    internal interface IModule
    {
        string Name { get; }
        IScreen UserInterface { get; }
    }
}

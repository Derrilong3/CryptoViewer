using Caliburn.Micro;

namespace CryptoViewer.Modules.MainMenu.Models
{
    internal interface IMenuItem
    {
        string Name { get; }
        IScreen UserInterface { get; }
    }
}

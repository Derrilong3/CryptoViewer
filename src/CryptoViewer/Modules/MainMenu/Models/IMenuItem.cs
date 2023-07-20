using Caliburn.Micro;

namespace CryptoViewer.Modules.MainMenu.Models
{
    internal interface IMenuItem
    {
        string ImagePath { get; }
        IScreen UserInterface { get; }
    }
}

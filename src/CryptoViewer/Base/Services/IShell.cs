using Caliburn.Micro;
using CryptoViewer.Modules.MainMenu;

namespace CryptoViewer.Base.Services
{
    internal interface IShell
    {
        IMenu MainMenu { get; }

        void ActivateItem(IScreen obj);
    }
}

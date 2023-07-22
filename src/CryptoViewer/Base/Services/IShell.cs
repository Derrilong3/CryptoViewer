using Caliburn.Micro;
using CryptoViewer.Modules.MainMenu;
using System.Threading.Tasks;

namespace CryptoViewer.Base.Services
{
    internal interface IShell
    {
        IScreen ActiveItem { get; }
        IMenu MainMenu { get; }

        Task ActivateItem(IScreen obj);
    }
}

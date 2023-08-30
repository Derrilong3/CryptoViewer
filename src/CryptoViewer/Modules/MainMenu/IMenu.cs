using CryptoViewer.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoViewer.Modules.MainMenu
{
    internal interface IMenu
    {
        List<IModule> Modules { get; }

        Task ClickMenu(IModule item);
    }
}

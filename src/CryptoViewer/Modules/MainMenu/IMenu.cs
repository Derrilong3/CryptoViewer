using CryptoViewer.Base;
using System.Collections.Generic;

namespace CryptoViewer.Modules.MainMenu
{
    internal interface IMenu
    {
        List<IModule> Modules { get; }

        void ClickMenu(IModule item);
    }
}

using CryptoViewer.Modules.MainMenu.Models;
using System.Collections.Generic;

namespace CryptoViewer.Modules.MainMenu
{
    internal interface IMenu
    {
        List<IMenuItem> Modules { get; }
        IMenuItem SelectedContent { get; }
    }
}

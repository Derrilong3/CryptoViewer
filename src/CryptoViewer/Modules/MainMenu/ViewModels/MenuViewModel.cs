using Caliburn.Micro;
using CryptoViewer.Base.Services;
using CryptoViewer.Modules.Home.ViewModels;
using CryptoViewer.Modules.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace CryptoViewer.Modules.MainMenu.ViewModels
{
    [Export(typeof(IMenu))]
    internal class MenuViewModel : Conductor<IScreen>, IMenu
    {
        public List<IMenuItem> Modules { get; }

        public MenuViewModel()
        {
            Modules = Assembly.GetAssembly(typeof(IMenuItem))
                .GetTypes()
                .Where(myType => !myType.IsAbstract && typeof(IMenuItem)
                .IsAssignableFrom(myType))
                .Select(t => (IMenuItem)Activator.CreateInstance(t))
                .ToList();
        }

        public void ClickMenu(IMenuItem item)
        {
            var shell = IoC.Get<IShell>();
            var selectedContent = item.UserInterface;

            if (selectedContent == shell.ActiveItem)
                return;

            shell.ActivateItem(selectedContent);
        }

        protected override void OnViewLoaded(object view)
        {
            ClickMenu(Modules.FirstOrDefault(x => x.UserInterface.GetType() == typeof(HomeViewModel)));
        }
    }
}

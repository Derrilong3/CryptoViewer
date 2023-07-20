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

        [ImportingConstructor]
        public MenuViewModel()
        {
            Modules = Assembly.GetAssembly(typeof(IMenuItem))
                .GetTypes()
                .Where(myType => !myType.IsAbstract && typeof(IMenuItem)
                .IsAssignableFrom(myType))
                .Select(t => (IMenuItem)Activator.CreateInstance(t))
                .ToList();
        }

        private IMenuItem _selectedContent;
        public IMenuItem SelectedContent
        {
            get => _selectedContent;
            set
            {
                if (value != _selectedContent)
                {
                    _selectedContent = value;

                    var shell = IoC.Get<IShell>();
                    shell.ActivateItem(_selectedContent.UserInterface);
                }
            }
        }

        protected override void OnViewLoaded(object view)
        {
            SelectedContent = Modules.FirstOrDefault(x => x.UserInterface.GetType() == typeof(HomeViewModel));
        }
    }
}

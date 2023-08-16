using Caliburn.Micro;
using CryptoViewer.Base;
using CryptoViewer.Base.Services;
using CryptoViewer.Modules.Home.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CryptoViewer.Modules.MainMenu.ViewModels
{
    [Export(typeof(IMenu))]
    internal class MenuViewModel : Conductor<IScreen>, IMenu
    {
        [Import]
        private IShell _shell;

        [ImportMany]
        private List<IModule> _modules;

        public List<IModule> Modules => _modules;

        public void ClickMenu(IModule item)
        {
            var selectedContent = item.UserInterface;

            if (selectedContent == _shell.ActiveItem)
                return;

            _shell.ActivateItem(selectedContent);
        }

        protected override void OnViewLoaded(object view)
        {
            ClickMenu(Modules.FirstOrDefault(x => x.UserInterface.GetType() == typeof(HomeViewModel)));
        }
    }
}

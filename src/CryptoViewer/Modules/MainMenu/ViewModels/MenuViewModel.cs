using Caliburn.Micro;
using CryptoViewer.Base;
using CryptoViewer.Base.Services;
using CryptoViewer.Modules.Home;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoViewer.Modules.MainMenu.ViewModels
{
    [Export(typeof(IMenu))]
    internal class MenuViewModel : Screen, IMenu
    {
        [Import]
        private IShell _shell;

        [ImportMany]
        private List<IModule> _modules;

        public List<IModule> Modules => _modules;

        public async Task ClickMenu(IModule item)
        {
            var selectedContent = item.UserInterface;

            await Open(selectedContent);
        }

        private async Task Open(IScreen screen)
        {
            if (screen == _shell.ActiveItem)
                return;

            await _shell.ActivateItemAsync(screen, CancellationToken.None);
        }

        protected override async void OnViewLoaded(object view)
        {
            await Open((IScreen)IoC.Get<IHome>());
        }
    }
}

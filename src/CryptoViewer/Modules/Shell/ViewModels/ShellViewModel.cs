using Caliburn.Micro;
using CryptoViewer.Base.Services;
using CryptoViewer.Modules.MainMenu;
using System.ComponentModel.Composition;

namespace CryptoViewer.Modules.Shell.ViewModels
{
    [Export(typeof(IShell))]
    internal class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        private IMenu _menu;
        public IMenu MainMenu => _menu;

        [ImportingConstructor]
        public ShellViewModel(IMenu menu)
        {
            _menu = menu;
        }

        public void ActivateItem(IScreen obj)
        {
            if (ReferenceEquals(obj, this))
                return;

            if (ReferenceEquals(obj, ActiveItem))
                return;

            ActivateItemAsync(obj);
        }
    }
}

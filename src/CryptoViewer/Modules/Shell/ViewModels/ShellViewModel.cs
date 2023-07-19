using Caliburn.Micro;
using CryptoViewer.Services;

namespace CryptoViewer.Modules.Shell.ViewModels
{
    internal class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell
    {
        public ShellViewModel()
        {
        }

        public void ActivateItem(IScreen obj)
        {
            if (ReferenceEquals(obj, ActiveItem))
                return;

            ActivateItemAsync(obj);
        }
    }
}

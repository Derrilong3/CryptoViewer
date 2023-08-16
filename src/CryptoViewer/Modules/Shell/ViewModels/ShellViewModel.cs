using Caliburn.Micro;
using CryptoViewer.Base.Events;
using CryptoViewer.Base.Services;
using CryptoViewer.Modules.MainMenu;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoViewer.Modules.Shell.ViewModels
{
    [Export(typeof(IShell))]
    internal class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShell, IHandle<ChangeActiveItemEvent>
    {
        [Import]
        private IMenu _menu;

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.SubscribeOnPublishedThread(this);
        }

        public IMenu MainMenu => _menu;

        public Task ActivateItem(IScreen obj)
        {
            if (ReferenceEquals(obj, this))
                return Task.CompletedTask;

            if (ReferenceEquals(obj, ActiveItem))
                return Task.CompletedTask;

            return ActivateItemAsync(obj);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            Properties.Settings.Default.Save();

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public Task HandleAsync(ChangeActiveItemEvent message, CancellationToken cancellationToken)
        {
            return ActivateItem(message.NewActiveItem);
        }
    }
}

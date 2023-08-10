using Caliburn.Micro;
using CryptoViewer.Base.Events;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Utilities.GridViewUtilities;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CryptoViewer.Modules.Home.ViewModels
{
    [Export(typeof(IHome))]
    internal class HomeViewModel : Screen, IHome
    {
        private readonly IApiHandler _apiHandler;
        private readonly IEventAggregator _eventAggregator;
        private bool _isDark;

        [ImportingConstructor]
        public HomeViewModel(IApiHandler apiHandler, IEventAggregator eventAggregator)
        {
            _apiHandler = apiHandler;
            _eventAggregator = eventAggregator;

            GridHandler = new GridViewHandler();
        }

        public GridViewHandler GridHandler { get; }

        private IEnumerable<IExchanger> _exchangers;
        public IEnumerable<IExchanger> Exchangers
        {
            get => _exchangers;
            private set
            {
                _exchangers = value;

                NotifyOfPropertyChange(nameof(Exchangers));
            }
        }

        private IExchanger _selectedExchanger;
        public IExchanger SelectedExchanger
        {
            get => _selectedExchanger;
            set
            {
                if (value == null)
                    return;

                _selectedExchanger = value;

                SetPairsAsync();

                NotifyOfPropertyChange(nameof(SelectedExchanger));
            }
        }

        private IEnumerable<IPair> _pairs;
        public IEnumerable<IPair> Pairs
        {
            get => _pairs ?? Enumerable.Empty<IPair>();
            private set
            {
                _pairs = value;

                NotifyOfPropertyChange(nameof(Pairs));
            }
        }

        private async Task SetPairsAsync()
        {
            Pairs = await _apiHandler.GetCurrenciesAsync(_selectedExchanger);

            if (GridHandler.Columns == null && _pairs.Any())
            {
                GridHandler.CreateGridColumns((string.Empty, _pairs.First().GetType()));
            }

            GridHandler.View = (CollectionView)CollectionViewSource.GetDefaultView(_pairs);
        }

        public async Task ShowDetailsAsync(IPair pair)
        {
            var coin = await _apiHandler.GetCurrencyAsync(pair.BaseId);

            var details = IoC.Get<IDetails>();
            details.Coin = coin;

            await _eventAggregator.PublishOnUIThreadAsync(new ChangeActiveItemEvent((IScreen)details));
        }

        public void ChangeTheme()
        {
            _isDark = !_isDark;
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            IBaseTheme baseTheme = _isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            paletteHelper.SetTheme(theme);
        }

        protected override async void OnViewLoaded(object view)
        {
            Exchangers = await _apiHandler.GetExchangersAsync();
            SelectedExchanger = _exchangers.First();
        }
    }
}

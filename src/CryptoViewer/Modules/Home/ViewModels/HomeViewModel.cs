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
                if (_selectedExchanger == value)
                    return;

                _selectedExchanger = value;

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

        public async Task ShowDetails(IPair pair)
        {
            var coin = await _apiHandler.GetCurrency(pair.BaseId);

            var details = IoC.Get<IDetails>();
            details.Coin = coin;

            await _eventAggregator.PublishOnUIThreadAsync(new ChangeActiveItemEvent((IScreen)details));
        }

        public void ChangeTheme()
        {
            _isDark = !_isDark;
            PaletteHelper _paletteHelper = new PaletteHelper();
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = _isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        protected override async void OnViewLoaded(object view)
        {
            Exchangers = await _apiHandler.GetExchangers();

            var exchanger = _exchangers.First();

            if (exchanger != null)
                SelectedExchanger = exchanger;

            Pairs = await _apiHandler.GetCurrencies(_selectedExchanger);

            if (GridHandler.Columns == null && _pairs.Any())
            {
                GridHandler.CreateGridColumns((string.Empty, _pairs.First().GetType()));
            }

            GridHandler.View = (CollectionView)CollectionViewSource.GetDefaultView(_pairs);
        }
    }
}

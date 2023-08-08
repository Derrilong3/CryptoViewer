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
    internal class HomeViewModel : Conductor<IScreen>, IHome
    {
        private IApiHandler _apiHandler;
        private IEventAggregator _eventAggregator;
        private bool _isDark = false;

        [ImportingConstructor]
        public HomeViewModel(IApiHandler apiHandler, IEventAggregator eventAggregator)
        {
            _apiHandler = apiHandler;
            _eventAggregator = eventAggregator;

            _gridHandler = new GridViewHandler();
        }

        private GridViewHandler _gridHandler;
        public GridViewHandler GridHandler => _gridHandler;

        private IEnumerable<IExchanger> _exchangers;
        public IEnumerable<IExchanger> Exchangers
        {
            get
            {
                if (_exchangers == null)
                {
                    GetExchangers();
                }

                return _exchangers;
            }
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

                GetPairs();

                NotifyOfPropertyChange(nameof(SelectedExchanger));
            }
        }

        public IEnumerable<IPair> _pairs;
        public IEnumerable<IPair> Pairs
        {
            get
            {
                if (_pairs == null)
                    return Enumerable.Empty<IPair>();

                return _pairs;
            }
            private set
            {
                _pairs = value;

                NotifyOfPropertyChange(nameof(Pairs));
            }
        }

        private async Task GetExchangers()
        {
            Exchangers = await _apiHandler.GetExchangers();

            var exchanger = _exchangers.First();

            if (exchanger != null)
                SelectedExchanger = exchanger;
        }

        private async Task GetPairs()
        {
            Pairs = await _apiHandler.GetCurrencies(_selectedExchanger);

            if (_gridHandler.Columns == null && _pairs.Count() > 0)
            {
                _gridHandler.CreateGridColumns((string.Empty, _pairs.First().GetType()));
            }

            _gridHandler.View = (CollectionView)CollectionViewSource.GetDefaultView(_pairs);
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
    }
}

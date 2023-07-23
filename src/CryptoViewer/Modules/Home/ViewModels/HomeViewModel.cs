using Caliburn.Micro;
using CryptoViewer.Base.Events;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Utilities.GridViewUtilities;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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
                    _exchangers = _apiHandler.GetExchangers();

                return _exchangers;
            }
        }

        private IExchanger _selectedExchanger;
        public IExchanger SelectedExchanger
        {
            get => _selectedExchanger;
            set
            {
                _selectedExchanger = value;
                Pairs = _apiHandler.GetCurrencies(_selectedExchanger);

                NotifyOfPropertyChange(nameof(SelectedExchanger));
            }
        }

        public IEnumerable<IPair> _pairs;
        public IEnumerable<IPair> Pairs
        {
            get => _pairs;
            set
            {
                _pairs = value;

                if (_gridHandler.View == null && _pairs.Count() > 0)
                {
                    _gridHandler.CreateGridColumns((string.Empty, _pairs.First().GetType()));
                }

                _gridHandler.View = (CollectionView)CollectionViewSource.GetDefaultView(_pairs);

                NotifyOfPropertyChange(nameof(Pairs));
            }
        }

        public void ShowDetails(IPair pair)
        {
            var coin = _apiHandler.GetCurrency(pair.BaseId);

            var details = IoC.Get<IDetails>();
            details.Coin = coin;

            _eventAggregator.PublishOnUIThreadAsync(new ChangeActiveItemEvent((IScreen)details));
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

        protected override void OnViewLoaded(object view)
        {
            var exchanger = Exchangers.First();
            if (exchanger == null)
                return;

            SelectedExchanger = exchanger;
        }
    }
}

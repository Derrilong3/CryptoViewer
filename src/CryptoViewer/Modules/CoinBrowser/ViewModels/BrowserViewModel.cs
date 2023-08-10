using Caliburn.Micro;
using CryptoViewer.Base.Events;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Utilities.GridViewUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CryptoViewer.Modules.CoinBrowser.ViewModels
{
    [Export(typeof(IBrowser))]
    internal class BrowserViewModel : Screen, IBrowser
    {
        private readonly IApiHandler _apiHandler;
        private readonly IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public BrowserViewModel(IApiHandler apiHandler, IEventAggregator eventAggregator)
        {
            _apiHandler = apiHandler;
            _eventAggregator = eventAggregator;

            GridHandler = new GridViewHandler();
        }

        public GridViewHandler GridHandler { get; }

        private IEnumerable<ICoin> _currencies;
        public IEnumerable<ICoin> Currencies
        {
            get => _currencies;
            private set
            {
                _currencies = value;

                NotifyOfPropertyChange(nameof(Currencies));
            }
        }

        public async Task ShowDetailsAsync(ICoin coin)
        {
            var details = IoC.Get<IDetails>();
            details.Coin = coin;

            await _eventAggregator.PublishOnUIThreadAsync(new ChangeActiveItemEvent((IScreen)details));
        }

        protected override async void OnViewLoaded(object view)
        {
            Currencies = await _apiHandler.GetCurrenciesAsync();

            if (!_currencies.Any())
                return;

            GridHandler.CreateGridColumns((string.Empty, _currencies
                .First()
                .GetType()
                ));

            GridHandler.View = (CollectionView)CollectionViewSource.GetDefaultView(_currencies);
            GridHandler.View.Filter = TextFilter;
        }

        #region Table filtring

        private string _searchFieldText;
        public string SearchFieldText
        {
            get => _searchFieldText;
            set
            {
                _searchFieldText = value;
                GridHandler.View.Refresh();
            }
        }

        private bool TextFilter(object item)
        {
            var coin = item as ICoin;
            var isNameMatch = string.IsNullOrEmpty(_searchFieldText) || coin.Name.IndexOf(_searchFieldText, StringComparison.OrdinalIgnoreCase) >= 0;
            var isCodeMatch = string.IsNullOrEmpty(_searchFieldText) || coin.Symbol.IndexOf(_searchFieldText, StringComparison.OrdinalIgnoreCase) >= 0;

            return isNameMatch || isCodeMatch;
        }

        #endregion
    }
}

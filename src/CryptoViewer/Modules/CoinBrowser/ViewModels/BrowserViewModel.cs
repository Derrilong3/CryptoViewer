using Caliburn.Micro;
using CryptoViewer.Base.Events;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Utilities.GridViewUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Data;

namespace CryptoViewer.Modules.CoinBrowser.ViewModels
{
    [Export(typeof(IBrowser))]
    internal class BrowserViewModel : Conductor<IScreen>, IBrowser
    {
        private IApiHandler _apiHandler;
        private IEventAggregator _eventAggregator;

        [ImportingConstructor]
        public BrowserViewModel(IApiHandler apiHandler, IEventAggregator eventAggregator)
        {
            _apiHandler = apiHandler;
            _eventAggregator = eventAggregator;

            _gridHandler = new GridViewHandler();
        }

        private GridViewHandler _gridHandler;
        public GridViewHandler GridHandler => _gridHandler;

        private IEnumerable<ICoin> _currencies;
        public IEnumerable<ICoin> Currencies
        {
            get
            {
                if (_currencies == null)
                {
                    _currencies = _apiHandler.GetCurrencies();

                    if (_currencies.Count() > 0)
                    {
                        _gridHandler.CreateGridColumns((string.Empty, _currencies.First().GetType()));
                        _gridHandler.View = (CollectionView)CollectionViewSource.GetDefaultView(_currencies);
                        _gridHandler.View.Filter = TextFilter;
                    }
                }

                return _currencies;
            }
        }

        public void ShowDetails(ICoin coin)
        {
            var details = IoC.Get<IDetails>();

            SelectedCoin = coin;
            details.Coin = coin;

            _eventAggregator.PublishOnUIThreadAsync(new ChangeActiveItemEvent((IScreen)details));
        }

        private ICoin _selectedCoin;
        public ICoin SelectedCoin
        {
            get => _selectedCoin;
            set
            {
                _selectedCoin = value;
                NotifyOfPropertyChange(nameof(SelectedCoin));
            }
        }

        #region Table filtring

        private string _searchFieldText;
        public string SearchFieldText
        {
            get => _searchFieldText;
            set
            {
                _searchFieldText = value;
                _gridHandler.View.Refresh();
            }
        }

        private bool TextFilter(object item)
        {
            var coin = item as ICoin;
            bool isNameMatch = string.IsNullOrEmpty(_searchFieldText) || coin.Name.IndexOf(_searchFieldText, StringComparison.OrdinalIgnoreCase) >= 0;
            bool isCodeMatch = string.IsNullOrEmpty(_searchFieldText) || coin.Symbol.IndexOf(_searchFieldText, StringComparison.OrdinalIgnoreCase) >= 0;

            return isNameMatch || isCodeMatch;
        }

        #endregion
    }
}

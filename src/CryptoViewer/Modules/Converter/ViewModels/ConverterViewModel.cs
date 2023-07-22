using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CryptoViewer.Modules.Converter.ViewModels
{
    [Export(typeof(IConverter))]
    internal class ConverterViewModel : Conductor<IScreen>, IConverter
    {
        private IApiHandler _apiHandler;

        [ImportingConstructor]
        public ConverterViewModel(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
        }


        private IEnumerable<ICoin> _currencies;
        public IEnumerable<ICoin> Currencies
        {
            get
            {
                if (_currencies == null)
                {
                    _currencies = _apiHandler.GetCurrencies();
                }

                return _currencies;
            }
        }

        private ICoin _firstCoin;
        public ICoin FirstCoin
        {
            get => _firstCoin;
            set
            {
                _firstCoin = value;

                NotifyOfPropertyChange(nameof(FirstCoin));
                NotifyOfPropertyChange(nameof(SecondAmount));
            }
        }

        private ICoin _secondCoin;
        public ICoin SecondCoin
        {
            get => _secondCoin;
            set
            {
                _secondCoin = value;

                NotifyOfPropertyChange(nameof(SecondCoin));
                NotifyOfPropertyChange(nameof(SecondAmount));
            }
        }

        private string _firstAmountText;
        public string FirstAmountText
        {
            get => _firstAmountText;
            set
            {
                float result;
                if (float.TryParse(value, out result))
                {
                    FirstAmount = result;
                }

                _firstAmountText = value;
            }
        }

        private float _firstAmount;
        public float FirstAmount
        {
            get => _firstAmount;
            set
            {
                _firstAmount = value;

                NotifyOfPropertyChange(nameof(SecondAmount));
            }
        }

        public string SecondAmount
        {
            get
            {
                if (_firstCoin == null || SecondCoin == null || _secondCoin.Price == 0)
                    return "";

                return ((_firstAmount * _firstCoin.Price) / _secondCoin.Price).ToString();
            }
        }

        protected override void OnViewLoaded(object view)
        {
            FirstCoin = Currencies.FirstOrDefault();
            SecondCoin = Currencies.FirstOrDefault();
        }
    }
}

using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Handlers.Models;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;

namespace CryptoViewer.Modules.Home.ViewModels
{
    [Export(typeof(IHome))]
    internal class HomeViewModel : Conductor<IScreen>, IHome
    {
        private IApiHandler _handler;

        [ImportingConstructor]
        public HomeViewModel(IApiHandler handler)
        {
            _handler = handler;
        }

        private IEnumerable<IExchanger> _exchangers;
        public IEnumerable<IExchanger> Exchangers
        {
            get
            {
                if (_exchangers == null)
                    _exchangers = _handler.GetExchangers();

                return _exchangers;
            }
        }

        private IExchanger _selectedExchanger;
        public IExchanger SelectedExchanger
        {
            get { return _selectedExchanger; }
            set
            {
                _selectedExchanger = value;
                NotifyOfPropertyChange(nameof(Pairs));
                NotifyOfPropertyChange(nameof(SelectedExchanger));
            }
        }

        public IEnumerable<InnerPair> Pairs => _selectedExchanger != null ? _handler.GetCurrencies(_selectedExchanger) : Enumerable.Empty<InnerPair>();

        protected override void OnViewLoaded(object view)
        {
            var exchanger = Exchangers.First();
            if (exchanger == null)
                return;

            SelectedExchanger = exchanger;
        }
    }
}

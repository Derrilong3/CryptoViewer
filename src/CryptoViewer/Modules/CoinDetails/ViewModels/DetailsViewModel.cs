using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Charts.LiveCharts;
using CryptoViewer.Utilities.GridViewUtilities;
using LiveCharts;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CryptoViewer.Modules.CoinDetails.ViewModels
{
    [Export(typeof(IDetails))]
    internal class DetailsViewModel : Conductor<IScreen>, IDetails
    {
        private IApiHandler _apiHandler;

        [ImportingConstructor]
        public DetailsViewModel(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
            _chart = new LiveChartsModel();
            _gridHandler = new GridViewHandler();
        }

        private GridViewHandler _gridHandler;
        public GridViewHandler GridView => _gridHandler;

        private LiveChartsModel _chart;
        public SeriesCollection Chart => _chart.SeriesCollection;
        public string[] Labels => _chart.Labels;

        public string Name => $"{Coin.Name} ({Coin.Symbol})";

        private ICoin _coin;
        public ICoin Coin
        {
            get => _coin;
            set
            {
                if (value == null || _coin == value)
                    return;

                _coin = value;

                GetPairs();

                NotifyOfPropertyChange(nameof(Coin));
                NotifyOfPropertyChange(nameof(Name));
            }
        }

        private IEnumerable<IPair> _pairs;
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

        private async Task GetPairs()
        {
            Pairs = await _apiHandler.GetExchangers(_coin);

            if (_gridHandler.Columns == null && _pairs.Count() > 0)
            {
                _gridHandler.CreateGridColumns((string.Empty, _pairs.First().GetType()));
            }

            _gridHandler.View = (CollectionView)CollectionViewSource.GetDefaultView(_pairs);
        }

        public async Task IntervalValue(string interval)
        {
            if (Coin == null)
                return;

            double[][] data = await _apiHandler.GetOHLC(Coin, interval);
            _chart.InitCandleStickChart(Coin.Name, data);

            NotifyOfPropertyChange(nameof(Labels));
        }

        protected override async void OnViewReady(object view)
        {
            await IntervalValue("1");
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _chart.Dispose();

            return base.OnDeactivateAsync(close, cancellationToken);
        }
    }
}

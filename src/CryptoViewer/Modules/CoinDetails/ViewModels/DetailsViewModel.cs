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
    internal class DetailsViewModel : Screen, IDetails
    {
        private readonly IApiHandler _apiHandler;
        private readonly LiveChartsModel _chart;

        [ImportingConstructor]
        public DetailsViewModel(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
            _chart = new LiveChartsModel();
            GridView = new GridViewHandler();
        }

        public GridViewHandler GridView { get; }
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

                SetPairsAsync();

                NotifyOfPropertyChange(nameof(Coin));
                NotifyOfPropertyChange(nameof(Name));
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
            Pairs = await _apiHandler.GetExchangersAsync(_coin);

            if (GridView.Columns == null && _pairs.Any())
            {
                GridView.CreateGridColumns((string.Empty, _pairs.First().GetType()));
            }

            GridView.View = (CollectionView)CollectionViewSource.GetDefaultView(_pairs);
        }

        public async Task IntervalValueAsync(string interval)
        {
            if (Coin == null)
                return;

            var data = await _apiHandler.GetOhlcAsync(Coin, interval);
            _chart.InitCandleStickChart(Coin.Name, data);

            NotifyOfPropertyChange(nameof(Labels));
        }

        protected override async void OnViewReady(object view)
        {
            await IntervalValueAsync("1");
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _chart.Dispose();

            return base.OnDeactivateAsync(close, cancellationToken);
        }
    }
}

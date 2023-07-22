using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Charts.LiveCharts;
using LiveCharts;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoViewer.Modules.CoinDetails.ViewModels
{
    [Export(typeof(IDetails))]
    internal class DetailsViewModel : Conductor<IScreen>, IDetails
    {
        private IApiHandler _apiHandler;

        public ICoin Coin { get; set; }

        [ImportingConstructor]
        public DetailsViewModel(IApiHandler apiHandler)
        {
            _apiHandler = apiHandler;
            _chart = new LiveChartsModel();
        }

        private LiveChartsModel _chart;
        public SeriesCollection Chart => _chart.SeriesCollection;
        public string[] Labels => _chart.Labels;

        public void IntervalValue(string interval)
        {
            double[][] data = _apiHandler.GetOHLC(Coin, interval);
            _chart.InitCandleStickChart(Coin.Name, data);
            NotifyOfPropertyChange(nameof(Labels));
        }

        protected override void OnViewReady(object view)
        {
            IntervalValue("1");
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _chart.Dispose();

            return base.OnDeactivateAsync(close, cancellationToken);
        }
    }
}

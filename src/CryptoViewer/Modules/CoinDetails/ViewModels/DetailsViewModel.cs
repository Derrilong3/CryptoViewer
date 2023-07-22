using Caliburn.Micro;
using CryptoViewer.Base.Interfaces;
using CryptoViewer.Base.Services;
using CryptoViewer.Charts.LiveCharts;
using LiveCharts;
using System.ComponentModel.Composition;

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

        protected override void OnViewReady(object view)
        {
            double[][] data = _apiHandler.GetOHLC(Coin, 30);
            _chart.InitCandleStickChart(data);
            NotifyOfPropertyChange(nameof(Labels));
        }
    }
}

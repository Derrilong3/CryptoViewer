using Caliburn.Micro;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;

namespace CryptoViewer.Charts.LiveCharts
{
    class LiveChartsModel : PropertyChangedBase
    {
        public SeriesCollection SeriesCollection { get; private set; }
        public string[] Labels { get; private set; }

        public LiveChartsModel()
        {
            SeriesCollection = new SeriesCollection();
        }

        public void InitCandleStickChart(double[][] data)
        {
            try
            {
                SeriesCollection.Clear();

                CandleSeries candles = new CandleSeries();
                candles.Values = new ChartValues<OhlcPoint>();

                string[] labels = new string[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    labels[i] = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(data[i][0].ToString())).ToString();
                    candles.Values.Add(new OhlcPoint(data[i][1], data[i][2], data[i][3], data[i][4]));
                }

                Labels = labels;
                SeriesCollection.Add(candles);
            }
            catch
            {
            }
        }

    }
}

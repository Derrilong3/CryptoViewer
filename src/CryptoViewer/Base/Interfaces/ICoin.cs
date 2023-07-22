namespace CryptoViewer.Base.Interfaces
{
    internal interface ICoin
    {
        string Id { get; set; }
        string Name { get; set; }
        int Rank { get; set; }
        float Price { get; set; }
        string Symbol { get; set; }
        float MarketCapUsd { get; set; }
        float Supply { get; set; }
        float VolumeUsd { get; set; }
        float ChangePercent { get; set; }
        float VWAP { get; set; }
    }
}

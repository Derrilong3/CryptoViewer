namespace CryptoViewer.Base.Interfaces
{
    internal interface IPair
    {
        string PairName { get; }
        string BaseId { get; set; }
        string BaseSymbol { get; set; }
        string ExhangeId { get; set; }
        float PriceUsd { get; set; }
        string QuoteId { get; set; }
        string QuoteSymbol { get; set; }
        float VolumeUsd { get; set; }
    }
}

﻿using CryptoViewer.Utilities.Attributes;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CryptoViewer.Handlers.Models
{
    internal class InnerPair : Pair
    {
        [ItemColumnData("Volume (%)", 150, "{0:0.##}")]
        [JsonProperty("percentExchangeVolume")]
        public float VolumePercent { get; set; }

        [ItemColumnData("Trades (24h)", 150, "{0:N0}")]
        [JsonProperty("tradesCount24Hr")]
        public int TradesCount { get; set; }  
    }
}

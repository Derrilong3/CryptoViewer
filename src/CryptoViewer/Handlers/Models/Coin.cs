﻿using CryptoViewer.Base.Interfaces;
using CryptoViewer.Utilities.Attributes;
using Newtonsoft.Json;

namespace CryptoViewer.Handlers.Models
{
    internal class Coin : ICoin
    {
        [ItemColumnData("Rank")]
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [ItemColumnData("Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [ItemColumnData("Price")]
        [JsonProperty("priceUsd")]
        public float Price { get; set; }

        [ItemColumnData("Supply")]
        [JsonProperty("supply")]
        public float Supply { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [ItemColumnData("Market Cap")]
        [JsonProperty("marketCapUsd")]
        public float MarketCapUsd { get; set; }

        [ItemColumnData("Volume (24Hr)")]
        [JsonProperty("volumeUsd24Hr")]
        public float VolumeUsd { get; set; }

        [ItemColumnData("Change (24Hr)")]
        [JsonProperty("changePercent24Hr")]
        public float ChangePercent { get; set; }

        [ItemColumnData("VWAP (24Hr)")]
        [JsonProperty("vwap24Hr")]
        public float VWAP { get; set; }
    }
}
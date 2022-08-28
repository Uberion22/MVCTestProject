﻿using Newtonsoft.Json;

namespace MVCTestProject.DataModels
{
    public class QuoteItem
    {
        [JsonProperty("price")]
        public double? Price { get; set; }

        [JsonProperty("volume_24h")]
        public double? Volume24h { get; set; }

        [JsonProperty("volume_change_24h")]
        public double? VolumeChange24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public double? PercentChange1h { get; set; }

        [JsonProperty("percent_change_24h")]
        public double? PercentChange24h { get; set; }

        [JsonProperty("percent_change_7d")]
        public double? PercentChange7d { get; set; }

        [JsonProperty("market_cap")]
        public double? MarketCap { get; set; }

        [JsonProperty("market_cap_dominance")]
        public double? MarketCapDominance { get; set; }

        [JsonProperty("fully_diluted_market_cap")]
        public double? FullyDilutedMarketCap { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
        
        public int QuoteId { get; set; }

        public virtual Quote Quote { get; set; }
    }
}
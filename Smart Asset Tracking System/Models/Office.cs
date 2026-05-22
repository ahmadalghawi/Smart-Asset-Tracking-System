using System;
using System.Collections.Generic;

namespace Smart_Asset_Tracking_System.Models
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        
        // Exchange rate from USD to local currency (e.g. 1 USD = 8.3333 SEK)
        public decimal ExchangeRateToUsd { get; set; } = 1.0m;

        // Navigation property for assets housed at this office
        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}

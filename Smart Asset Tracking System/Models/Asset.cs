using System;

namespace Smart_Asset_Tracking_System.Models
{
    public abstract class Asset
    {
        public int Id { get; set; }
        
        // E.g., "Laptop", "Desktop", "iPhone", "Tablet", etc.
        public string AssetType { get; set; } = string.Empty;
        
        public string Brand { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePriceUsd { get; set; }
        public string SerialNumber { get; set; } = string.Empty;

        // Foreign Key & Navigation to Office
        public int OfficeId { get; set; }
        public Office? Office { get; set; }

        // Optional Foreign Key & Navigation to Assigned Employee (Level 5)
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        // Maintenance Fields (Level 5)
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? MaintenanceNotes { get; set; }

        // Calculated Property: Expiration Date (3 years lifetime)
        public DateTime WarrantyExpirationDate => PurchaseDate.AddYears(3);

        // Calculated Property: Local price today based on office exchange rate
        public decimal LocalPrice => PurchasePriceUsd * (Office?.ExchangeRateToUsd ?? 1.0m);

        // Calculated Property: Remaining lifetime in months
        public double RemainingLifetimeMonths
        {
            get
            {
                var remainingDays = (WarrantyExpirationDate - DateTime.Today).TotalDays;
                return remainingDays / 30.4375; // average days in a month (365.25 / 12)
            }
        }

        // Returns Status based on requirements:
        // * YELLOW => warning = less than 3 months remaining
        // * RED => warning = less than 6 months remaining
        // * NORMAL => more than 6 months remaining
        public string Status
        {
            get
            {
                double months = RemainingLifetimeMonths;
                if (months < 3)
                {
                    return "YELLOW";
                }
                else if (months < 6)
                {
                    return "RED";
                }
                else
                {
                    return "NORMAL";
                }
            }
        }
    }
}

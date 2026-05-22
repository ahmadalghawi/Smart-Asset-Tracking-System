using System;
using System.Collections.Generic;

namespace Smart_Asset_Tracking_System.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation property for assets assigned to this employee
        public ICollection<Asset> AssignedAssets { get; set; } = new List<Asset>();
    }
}

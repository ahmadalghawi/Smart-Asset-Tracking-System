using System;

namespace Smart_Asset_Tracking_System.Models
{
    public class MobileAsset : Asset
    {
        public string SimCardNumber { get; set; } = string.Empty;
        public bool Is5gEnabled { get; set; }
    }
}

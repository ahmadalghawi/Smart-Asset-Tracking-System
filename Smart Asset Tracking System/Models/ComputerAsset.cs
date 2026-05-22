using System;

namespace Smart_Asset_Tracking_System.Models
{
    public class ComputerAsset : Asset
    {
        public string ProcessorType { get; set; } = string.Empty;
        public int RamSizeGb { get; set; }
    }
}

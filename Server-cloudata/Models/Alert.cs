using BuisnessLogic.Collector.Enums;
using Server_cloudata.Services;
using System;

namespace Server_cloudata.Models
{
    public class Alert
    {
        public string VMName { get; set; }
        public eNodeExporterData ThresholdKey { get; set; }
        public double ThresholdValue { get; set; }
        public double CurrentValue { get; set; }
        public DateTime Timestamp { get; set; }
        public string EmailRecipient { get; set; }

        //public string Description { get; set; }
    }
}

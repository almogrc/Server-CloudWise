using BuisnessLogic.Collector.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Server_cloudata.Enums;
using Server_cloudata.Services;
using System;
using System.Security.Policy;

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
       // public eAlertType AlertType { get; set; }
        //public string Description { get; set; }
    }
}

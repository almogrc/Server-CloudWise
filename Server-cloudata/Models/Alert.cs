using BuisnessLogic.Collector.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Server_cloudata.Enums;
using Server_cloudata.Models.PrometheusAlerts;
using Server_cloudata.Services;
using System;
using System.Security.Policy;

namespace Server_cloudata.Models
{
    public class Alert
    {
        public Alert() { }
        public Alert(AlertPrometheus alertPrometheus, VirtualMachine virtualMachine)
        {
            VMName = virtualMachine.Name;
            AlertName = alertPrometheus.Labels.AlertName;
            Title = alertPrometheus.Annotations.Title;
            Description = alertPrometheus.Annotations.Description;
            StartTime = alertPrometheus.StartsAt;
            Severity = alertPrometheus.Labels.Severity;
        }

        public string VMName { get; set; }
        public string AlertName { get; set; }
        public double ThresholdValue { get; set; }
        public double CurrentValue { get; set; }
        public string Title {  get; set; }  
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public string Severity { get; set; }
        public string EmailRecipient { get; set; }
        // public eAlertType AlertType { get; set; }
        //public string Description { get; set; }
    }
}

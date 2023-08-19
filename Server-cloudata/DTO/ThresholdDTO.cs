using BuisnessLogic.Collector.Enums;
using Server_cloudata.Models;
using System.Text.Json.Serialization;

namespace Server_cloudata.DTO
{
    public class ThresholdDTO : Threshold
    {
        public string Name { get; set; } 
    }
}

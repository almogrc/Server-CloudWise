using BuisnessLogic.Collector.Enums;
using Server_cloudata.Models;
using System.Collections.Generic;

namespace Server_cloudata.DTO
{
    public class NodeThresholdUpdateDTO
    {
        public Dictionary<eNodeExporterData, Threshold> thresholds { get; set; }
    }
}

using BuisnessLogic.Collector.Enums;

namespace Server_cloudata.DTO
{
    public class ThresholdDTO
    {
        public class ThresholdRequest
        {
            public string MachineName { get; set; }
            public eNodeExporterData Key { get; set; }
            public double Value { get; set; }
        }
    }
}

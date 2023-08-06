using BuisnessLogic.Collector.Enums;
using System.Text.Json.Serialization;

namespace Server_cloudata.DTO
{
    public class ThresholdDTO
    {
        public class ThresholdRequest
        {
            public string MachineName { get; set; }

            [JsonConverter(typeof(JsonStringEnumConverter))]
            public eNodeExporterData Key { get; set; }
            public double Value { get; set; }
        }
    }
}

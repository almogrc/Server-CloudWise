using Newtonsoft.Json;

namespace Server_cloudata.Models.PrometheusAlerts
{
    public class GroupLabels
    {
        [JsonProperty("alertname")]
        public string AlertName { get; set; }
    }
}

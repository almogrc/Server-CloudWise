using Newtonsoft.Json;

namespace Server_cloudata.Models.PrometheusAlerts
{
    public class LabelSet
    {
        [JsonProperty("alertname")]
        public string AlertName { get; set; }

        [JsonProperty("instance")]
        public string Instance { get; set; }

        [JsonProperty("job")]
        public string Job { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }
    }
}

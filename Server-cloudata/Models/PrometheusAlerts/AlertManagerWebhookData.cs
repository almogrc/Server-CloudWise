using Newtonsoft.Json;
using static Server_cloudata.Controllers.AlertHandlerController;

namespace Server_cloudata.Models.PrometheusAlerts
{
    public class AlertManagerWebhookData
    {
        [JsonProperty("receiver")]
        public string Receiver { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("alerts")]
        public AlertPrometheus[] Alerts { get; set; }

        [JsonProperty("groupLabels")]
        public GroupLabels GroupLabels { get; set; }

        [JsonProperty("commonLabels")]
        public CommonLabels CommonLabels { get; set; }

        [JsonProperty("commonAnnotations")]
        public CommonAnnotations CommonAnnotations { get; set; }

        [JsonProperty("externalURL")]
        public string ExternalURL { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("groupKey")]
        public string GroupKey { get; set; }

        [JsonProperty("truncatedAlerts")]
        public int TruncatedAlerts { get; set; }
    }
}

using Newtonsoft.Json;

namespace Server_cloudata.Models.PrometheusAlerts
{
    public class AnnotationSet
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}

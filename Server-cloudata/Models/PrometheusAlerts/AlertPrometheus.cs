using Newtonsoft.Json;
using System;
using static Server_cloudata.Controllers.AlertHandlerController;

namespace Server_cloudata.Models.PrometheusAlerts
{
    public class AlertPrometheus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("labels")]
        public LabelSet Labels { get; set; }

        [JsonProperty("annotations")]
        public AnnotationSet Annotations { get; set; }

        [JsonProperty("startsAt")]
        public DateTime StartsAt { get; set; }

        [JsonProperty("endsAt")]
        public string EndsAt { get; set; }

        [JsonProperty("generatorURL")]
        public string GeneratorURL { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }
    }
}

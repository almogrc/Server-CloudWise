using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Server_cloudata.Enums;
using System.Security.Policy;

namespace Server_cloudata.DTO
{
    public class VirtualMachineDTO
    {
        public string Name { get; set; }
        public string Supplier { get; set; }
        public string DNSAddress { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Status { get; set; }
    }
}

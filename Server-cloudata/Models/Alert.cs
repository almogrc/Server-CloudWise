using Server_cloudata.Services;
using System;

namespace Server_cloudata.Models
{
    public class Alert
    {
        public string VmName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
    }
}

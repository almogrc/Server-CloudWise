using Server_cloudata.Enums;
using System.Net;

namespace Server_cloudata.DTO
{
    public class NewMachineDTO : BaseBody
    {
        public string Name { get; set; }
        public Supplier Supplier { get; set; }
        public string DNSAddress { get; set; }
    }
}

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Server_cloudata.Enums;

namespace Server_cloudata.Models

{
    public class VirtualMachine
    {
        public string Name { get; set; }
        public string Supplier { get; set; }
        public string Address { get; set; }
    }
}

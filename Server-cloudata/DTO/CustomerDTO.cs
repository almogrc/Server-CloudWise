using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Server_cloudata.Models;
using System.Collections.Generic;

namespace Server_cloudata.DTO
{
    public class CustomerDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Server_cloudata.Models;
using System.Collections.Generic;

namespace Server_cloudata.DTO
{
    public class SignUpDTO
    {
        public string CustomerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}

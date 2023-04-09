using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Server_cloudata.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("email")]
        public string Email { get; set; } = null!;

        [BsonElement("password")]
        public string Password { get; set; } = null!;
    }
}


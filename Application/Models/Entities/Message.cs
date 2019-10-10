using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Models.Entities
{
    public class Message
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string ContactId { get; set; }
        public string message { get; set; }
        public long Timestamp { get; set; }
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Receiver.MongoServices.Models
{
    
    public class MongoMessage
    {
        public ObjectId Id { get; set; }
        [BsonElement("MessageId")]
        public int MessageId { get; set; }
        [BsonElement("Comment")]
        public string Comment { get; set; }
        [BsonElement("ErrorCount")]
        public int ErrorCount { get; set; }
    }
}

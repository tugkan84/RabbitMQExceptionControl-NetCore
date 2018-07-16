using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Receiver.MongoServices.Models
{
    // public class Product
    // {
    //     public ObjectId Id { get; set; }
    //     [BsonElement("ProductId")]
    //     public int ProductId { get; set; }
    //     [BsonElement("ProductName")]
    //     public string ProductName { get; set; }
    //     [BsonElement("Price")]
    //     public int Price { get; set; }
    //     [BsonElement("Category")]
    //     public string Category { get; set; }
    // }
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
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using Receiver.MongoServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Receiver.MongoServices.Services
{
    public class DataAccess
    {
        private readonly IMongoDatabase _db = null;

        public DataAccess()
        {
            var client = new MongoClient("mongodb://localhost:27017/myproject");
        if (client != null)
            _db = client.GetDatabase("messagemongodb");
        }

        public Task<List<MongoMessage>> GetMessages()
        {
            return _db.GetCollection<MongoMessage>("Message")
                    .Find(_ => true).ToListAsync();
        }

        public MongoMessage Create(MongoMessage p)
        {
            _db.GetCollection<MongoMessage>("Message").InsertOneAsync(p);
            return p;
        }

        
        public void RemoveAll(){
            _db.GetCollection<MongoMessage>("Message").DeleteManyAsync(new BsonDocument());
        }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HungryPizza.Infra.Data.MongoDB.Collections
{
    public class BaseCollection
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public int Id { get; set; }
    }
}

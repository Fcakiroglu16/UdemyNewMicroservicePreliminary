using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UdemyMicroservices.Catalog.Repositories
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
    }
}
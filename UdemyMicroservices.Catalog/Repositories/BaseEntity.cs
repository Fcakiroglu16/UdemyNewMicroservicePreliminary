using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UdemyMicroservices.Catalog.Repositories
{
    public abstract class BaseEntity
    {
        [BsonElement("_id")] public Guid Id { get; set; }
    }
}
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using UdemyMicroservices.Catalog.Repositories;

namespace UdemyMicroservices.Catalog.Features.Categories
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;
    }
}
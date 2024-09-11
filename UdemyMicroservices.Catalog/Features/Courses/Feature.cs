using MongoDB.Bson.Serialization.Attributes;

namespace UdemyMicroservices.Catalog.Features.Courses
{
    public class Feature
    {
        public int Duration { get; set; }

        public int Rating { get; set; }
    }
}
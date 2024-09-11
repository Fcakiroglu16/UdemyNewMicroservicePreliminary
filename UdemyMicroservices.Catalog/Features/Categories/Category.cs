using UdemyMicroservices.Catalog.Features.Courses;
using UdemyMicroservices.Catalog.Repositories;

namespace UdemyMicroservices.Catalog.Features.Categories;

public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public List<Course>? Courses { get; set; }
}
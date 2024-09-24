using UdemyMicroservices.Catalog.Features.Categories;
using UdemyMicroservices.Catalog.Repositories;

namespace UdemyMicroservices.Catalog.Features.Courses;

public class Course : BaseEntity
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;


    public decimal Price { get; set; }

    public string UserId { get; set; } = default!;

    public string? Picture { get; set; }


    public DateTime CreatedTime { get; set; }

    public Feature? Feature { get; set; }

    public Guid CategoryId { get; set; } = default!;

    public Category? Category { get; set; }
}
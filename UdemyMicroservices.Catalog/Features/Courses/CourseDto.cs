using UdemyMicroservices.Catalog.Features.Categories;

namespace UdemyMicroservices.Catalog.Features.Courses;

public record CourseDto(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string UserId,
    string Picture,
    DateTime CreatedTime,
    FeatureDto Feature,
    CategoryDto Category);
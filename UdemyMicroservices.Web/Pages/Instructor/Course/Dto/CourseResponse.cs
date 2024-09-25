namespace UdemyMicroservices.Web.Pages.Instructor.Course.Dto;

public record CourseResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    Guid UserId,
    string? Picture,
    DateTime CreatedTime,
    FeatureResponse Feature,
    CategoryResponse Category);
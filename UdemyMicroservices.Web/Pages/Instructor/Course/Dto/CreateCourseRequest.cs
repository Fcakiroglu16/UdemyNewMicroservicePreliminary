namespace UdemyMicroservices.Web.Pages.Instructor.CreateCourse;

public record CreateCourseRequest(
    string Name,
    string Description,
    decimal Price,
    string Picture,
    Guid CategoryId);
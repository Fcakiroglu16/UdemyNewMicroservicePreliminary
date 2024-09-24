namespace UdemyMicroservices.Web.Pages.Instructor.Course.Dto
{
    public record UpdateCourseRequest(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string? Picture,
        Guid CategoryId);
}
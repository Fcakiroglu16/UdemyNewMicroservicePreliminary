namespace UdemyMicroservices.Web.Pages.Basket.Dto
{
    public record AddBasketRequest(
        Guid CourseId,
        string CourseName,
        string? CoursePicture,
        decimal CoursePrice);
}
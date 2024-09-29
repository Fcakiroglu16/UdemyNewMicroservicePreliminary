namespace UdemyMicroservices.Web.Pages.Basket.Dto;

public record BasketItemDto(
    Guid CourseId,
    string CourseName,
    string? CoursePictureUrl,
    decimal CoursePrice,
    decimal? CoursePriceByApplyDiscountRate)
{
}
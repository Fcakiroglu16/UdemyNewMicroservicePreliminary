namespace UdemyMicroservices.Web.Pages.Basket.Dto;

public record BasketItemDto(
    Guid Id,
    string Name,
    string? PictureUrl,
    decimal Price,
    decimal? PriceByApplyDiscountRate,
    decimal CurrentPrice)
{
}
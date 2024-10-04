namespace UdemyMicroservices.Basket.Dto;

public record BasketItemDto(
    Guid Id,
    string Name,
    string? PictureUrl,
    decimal Price,
    decimal? PriceByApplyDiscountRate)
{
    public decimal CurrentPrice => PriceByApplyDiscountRate ?? Price;
}
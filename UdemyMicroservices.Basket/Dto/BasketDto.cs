namespace UdemyMicroservices.Basket.Dto;

public record BasketDto(string UserId, int? DiscountRate, List<BasketItemDto> BasketItems)
{
    public decimal TotalPrice => BasketItems.Sum(x => x.Price);
}
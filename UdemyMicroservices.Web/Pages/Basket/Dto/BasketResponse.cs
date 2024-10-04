namespace UdemyMicroservices.Web.Pages.Basket.Dto;

public record BasketResponse(
    string UserId,
    string? Coupon,
    float? DiscountRate,
    decimal? TotalPriceByApplyDiscountRate,
    decimal TotalPrice,
    decimal CurrentTotalPrice,
    List<BasketItemDto> BasketItems);
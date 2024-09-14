using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket;

public record SaveOrUpdateBasketCommand(string UserId, int? DiscountRate, List<BasketItemDto> BasketItems)
    : IRequestByServiceResult;
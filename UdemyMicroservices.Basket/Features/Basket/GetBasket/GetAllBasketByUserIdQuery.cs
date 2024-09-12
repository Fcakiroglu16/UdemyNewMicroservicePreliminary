using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.GetBasket
{
    public record GetAllBasketByUserIdQuery : IRequestByServiceResult<BasketDto>;
}
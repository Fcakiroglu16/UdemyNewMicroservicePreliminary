using MassTransit;
using MediatR;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket
{
    public record SaveOrUpdateBasketCommand(BasketDto Basket) : IRequestByServiceResult;
}
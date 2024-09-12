using MassTransit;
using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.DeleteBasket
{
    public record DeleteBasketCommand(string UserId) : IRequestByServiceResult;
}
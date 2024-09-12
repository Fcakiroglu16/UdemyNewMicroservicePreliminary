using MediatR;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket;

public static class SaveOrUpdateBasketEndpoint
{
    public static RouteGroupBuilder MapSaveOrUpdateBasketEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IMediator mediator, BasketDto basket) =>
                {
                    var command = new SaveOrUpdateBasketCommand(basket);
                    var result = await mediator.Send(command);
                    return result.ToActionResult();
                })
            .WithName("SaveOrUpdateBasket")
            .Produces(StatusCodes.Status204NoContent)
            .MapToApiVersion(1.0);

        return group;
    }
}
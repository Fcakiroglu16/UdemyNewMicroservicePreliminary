using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.AddBasket;

public static class AddBasketEndpoint
{
    public static RouteGroupBuilder MapAddBasketEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IMediator mediator, AddBasketCommand addBasketCommand) =>
                {
                    var result = await mediator.Send(addBasketCommand);
                    return result.ToActionResult();
                })
            .WithName("SaveOrUpdateBasket")
            .Produces(StatusCodes.Status204NoContent)
            .MapToApiVersion(1.0)
            .AddEndpointFilter<ValidationFilter<AddBasketCommand>>();

        return group;
    }
}
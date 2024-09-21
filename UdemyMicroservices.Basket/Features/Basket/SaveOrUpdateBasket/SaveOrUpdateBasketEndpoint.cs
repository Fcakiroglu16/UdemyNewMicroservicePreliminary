using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket;

public static class SaveOrUpdateBasketEndpoint
{
    public static RouteGroupBuilder MapSaveOrUpdateBasketEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IMediator mediator, SaveOrUpdateBasketCommand saveOrUpdateBasketCommand) =>
                {
                    var result = await mediator.Send(saveOrUpdateBasketCommand);
                    return result.ToActionResult();
                })
            .WithName("SaveOrUpdateBasket")
            .Produces(StatusCodes.Status204NoContent)
            .MapToApiVersion(1.0)
            .AddEndpointFilter<ValidationFilter<SaveOrUpdateBasketCommand>>();

        return group;
    }
}
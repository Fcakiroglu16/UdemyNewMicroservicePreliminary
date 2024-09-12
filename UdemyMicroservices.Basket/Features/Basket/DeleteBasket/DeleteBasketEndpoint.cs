using MassTransit;
using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.DeleteBasket
{
    public static class DeleteBasketEndpoint
    {
        public static RouteGroupBuilder MapDeleteBasketEndpoint(this RouteGroupBuilder group)
        {
            group.MapDelete("/{userId}",
                    async (IMediator mediator, string userId) =>
                    {
                        var command = new DeleteBasketCommand(userId);
                        var result = await mediator.Send(command);
                        return result.ToActionResult();
                    })
                .WithName("DeleteBasket")
                .Produces(StatusCodes.Status204NoContent)
                .MapToApiVersion(1.0);

            return group;
        }
    }
}
using MediatR;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.GetBasket
{
    public static class GetAllBasketByUserIdEndpoint
    {
        public static RouteGroupBuilder MapGetAllBasketByUserIdEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/",
                    async (IMediator mediator) =>
                        (await mediator.Send(new GetAllBasketByUserIdQuery())).ToActionResult())
                .WithName("GetCategoryById")
                .Produces<List<BasketDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound).MapToApiVersion(1.0);
            return group;
        }
    }
}
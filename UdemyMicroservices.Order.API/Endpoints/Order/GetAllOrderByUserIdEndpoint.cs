using MediatR;
using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Order.Application.Features.Orders.GetAllOrderByUserId;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Order.API.Endpoints.Order;

public static class GetAllOrderByUserIdEndpoint
{
    public static RouteGroupBuilder MapGetAllOrderByUserIdEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/userId",
                async ([FromServices] IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetAllOrderByUserIdQuery());
                    return result.ToActionResult();
                })
            .WithName("GetAllOrderByUserId")
            .Produces(StatusCodes.Status200OK)
            .MapToApiVersion(1.0);

        return group;
    }
}
using MassTransit;
using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Discount.Features.Discount.GetDiscountByCode;

public static class GetDiscountByCodeEndpoint
{
    public static RouteGroupBuilder MapDiscountByCodeEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{discountCode}",
                async (IMediator mediator, string discountCode) =>
                {
                    var query = new GetDiscountByCodeQuery(discountCode);
                    var result = await mediator.Send(query);
                    return result.ToActionResult();
                })
            .WithName("GetDiscountByCode")
            .Produces<Response<GetDiscountByCodeQueryResponse>>()
            .Produces(StatusCodes.Status404NotFound)
            .MapToApiVersion(1.0);


        return group;
    }
}
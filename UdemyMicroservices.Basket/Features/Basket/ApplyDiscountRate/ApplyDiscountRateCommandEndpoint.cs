using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.ApplyDiscountRate;

public static class ApplyDiscountRateCommandEndpoint
{
    public static RouteGroupBuilder MapApplyDiscountRateCommandEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/ApplyDiscountRate/{coupon}/{discountRate:float}",
                async (IMediator mediator, string coupon, float discountRate) =>
                    (await mediator.Send(new ApplyDiscountRateCommand(coupon, discountRate))).ToActionResult())
            .WithName("ApplyDiscountRate")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).MapToApiVersion(1.0);
        return group;
    }
}
using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Basket.Features.Basket.ApplyDiscountRate;

public static class ApplyDiscountCouponCommandEndpoint
{
    public static RouteGroupBuilder MapApplyDiscountRateCommandEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/ApplyDiscountCoupon/{coupon}/{discountRate:float}",
                async (IMediator mediator, string coupon, float discountRate) =>
                    (await mediator.Send(new ApplyDiscountCouponCommand(coupon, discountRate))).ToActionResult())
            .WithName("ApplyDiscountCoupon")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).MapToApiVersion(1.0);
        return group;
    }
}
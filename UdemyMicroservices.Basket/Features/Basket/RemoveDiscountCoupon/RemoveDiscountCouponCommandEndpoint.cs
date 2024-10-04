using MediatR;

namespace UdemyMicroservices.Basket.Features.Basket.RemoveDiscountCoupon;

public static class RemoveDiscountCouponCommandEndpoint
{
    public static RouteGroupBuilder MapARemoveDiscountCouponCommandEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/RemoveDiscountCoupon",
                async (IMediator mediator) =>
                    await mediator.Send(new RemoveDiscountCouponCommand()))
            .WithName("RemoveDiscountCoupon")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).MapToApiVersion(1.0);


        return group;
    }
}
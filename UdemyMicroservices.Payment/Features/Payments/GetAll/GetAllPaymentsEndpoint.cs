using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Payment.Features.Payments.GetAll;

public static class GetAllPaymentsEndpoint
{
    public static RouteGroupBuilder MapGetAllPaymentsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetAllPaymentsQuery());

                    return result.ToActionResult();
                })
            .WithName("GetAllPayments")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1.0);

        return group;
    }
}
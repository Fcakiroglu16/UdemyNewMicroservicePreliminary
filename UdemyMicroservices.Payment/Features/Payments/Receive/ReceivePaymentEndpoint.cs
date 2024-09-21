using FluentValidation;
using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Payment.Features.Payments.Receive;

public static class ReceivePaymentEndpoint
{
    public static RouteGroupBuilder MapReceivePaymentEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/receive",
                async (IMediator mediator, ReceivePaymentCommand command,
                    IValidator<ReceivePaymentCommand> validator) =>
                {
                    var result2 = await validator.ValidateAsync(command);

                    var result = await mediator.Send(command);

                    return result.ToActionResult();
                })
            .WithName("ReceivePayment")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .MapToApiVersion(1.0)
            .AddEndpointFilter<ValidationFilter<ReceivePaymentCommand>>();
        return group;
    }
}
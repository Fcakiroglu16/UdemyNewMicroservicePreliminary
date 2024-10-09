using MassTransit;
using MediatR;
using UdemyMicroservices.Discount.Repositories;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Discount.Features.Discount.CreateDiscount;

public record CreateDiscountCommand(string Code, int Rate, Guid UserId, DateTime Expired)
    : IRequestByServiceResult;

public class CreateDiscountCommandHandler(AppDbContext context)
    : IRequestHandler<CreateDiscountCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = new Repositories.Discount
        {
            Id = NewId.NextGuid(),
            UserId = request.UserId,
            Code = request.Code,
            Rate = request.Rate / 100f,
            Created = DateTime.Now,
            Expired = request.Expired
        };

        await context.Discounts.AddAsync(discount, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public static class CreateDiscountEndpoint
{
    public static RouteGroupBuilder MapCreateDiscountEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IMediator mediator, CreateDiscountCommand command) =>
                {
                    var result = await mediator.Send(command);
                    return result.ToActionResult();
                })
            .WithName("CreateDiscount")
            .Produces(StatusCodes.Status200OK)
            .MapToApiVersion(1.0)
            .AddEndpointFilter<ValidationFilter<CreateDiscountCommand>>();

        return group;
    }
}
using FluentValidation;
using UdemyMicroservices.Order.Application.Features.Orders.Dto;

namespace UdemyMicroservices.Order.Application.Features.Orders.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.DiscountRate)
            .GreaterThanOrEqualTo(0).When(x => x.DiscountRate.HasValue)
            .WithMessage("{PropertyName} must be a positive number or zero");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("{PropertyName} is required")
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("{PropertyName} must contain at least one order item");

        RuleForEach(x => x.OrderItems)
            .SetValidator(new OrderItemDtoValidator());
    }
}

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Line)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");

        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Matches(@"^\d{5}$").WithMessage("{PropertyName} must be 5 digits");
    }
}

public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");
    }
}
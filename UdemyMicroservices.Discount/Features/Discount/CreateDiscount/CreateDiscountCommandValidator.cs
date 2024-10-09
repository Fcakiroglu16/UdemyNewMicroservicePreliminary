using FluentValidation;

namespace UdemyMicroservices.Discount.Features.Discount.CreateDiscount;

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");


        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty")
            .Length(5, 20).WithMessage("{PropertyName} must be between 5 and 20 characters");

        RuleFor(x => x.Rate)
            .InclusiveBetween(1, 100).WithMessage("{PropertyName} must be between 1 and 100");

        RuleFor(x => x.Expired)
            .GreaterThan(DateTime.Now).WithMessage("{PropertyName} must be a future date");
    }
}
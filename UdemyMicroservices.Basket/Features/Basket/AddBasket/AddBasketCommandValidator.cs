using FluentValidation;

namespace UdemyMicroservices.Basket.Features.Basket.AddBasket;

public class AddBasketCommandValidator : AbstractValidator<AddBasketCommand>
{
    public AddBasketCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");

        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("{PropertyName} cannot be empty");


        RuleFor(x => x.CoursePrice)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");
    }
}
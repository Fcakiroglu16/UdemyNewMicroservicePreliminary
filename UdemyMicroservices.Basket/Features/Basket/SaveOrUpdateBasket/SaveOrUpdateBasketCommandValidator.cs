using FluentValidation;
using UdemyMicroservices.Basket.Dto;

namespace UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket
{
    public class SaveOrUpdateBasketCommandValidator : AbstractValidator<SaveOrUpdateBasketCommand>
    {
        public SaveOrUpdateBasketCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.DiscountRate)
                .GreaterThanOrEqualTo(0).When(x => x.DiscountRate.HasValue)
                .WithMessage("{PropertyName} must be greater than or equal to 0");

            RuleFor(x => x.BasketItems)
                .NotEmpty().WithMessage("{PropertyName} must contain at least one item");

            RuleForEach(x => x.BasketItems)
                .SetValidator(new BasketItemDtoValidator());
        }
    }

    public class BasketItemDtoValidator : AbstractValidator<BasketItemDto>
    {
        public BasketItemDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.CourseName)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero");
        }
    }
}
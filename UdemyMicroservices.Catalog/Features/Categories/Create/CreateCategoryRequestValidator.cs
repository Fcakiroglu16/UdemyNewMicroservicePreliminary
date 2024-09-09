using FluentValidation;

namespace UdemyMicroservices.Catalog.Features.Categories.Create
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");
        }
    }
}
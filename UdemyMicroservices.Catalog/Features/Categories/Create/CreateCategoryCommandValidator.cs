using FluentValidation;

namespace UdemyMicroservices.Catalog.Features.Categories.Create
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(3, 50).WithMessage("{PropertyName} must be between 3 and 50 characters");
        }
    }
}
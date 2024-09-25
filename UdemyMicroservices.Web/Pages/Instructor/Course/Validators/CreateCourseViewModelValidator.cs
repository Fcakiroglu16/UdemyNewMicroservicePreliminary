using FluentValidation;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;

namespace UdemyMicroservices.Web.Pages.Instructor.Course.Validators;

public class CreateCourseViewModelValidator : AbstractValidator<CreateCourseViewModel>
{
    public CreateCourseViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(5, 100).WithMessage("{PropertyName} must be between 5 and 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(5, 500).WithMessage("{PropertyName} must be between 5 and 100 characters");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .InclusiveBetween(0, 1000).WithMessage("Course price must be between 0 and 1000.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Category is required.");
    }
}
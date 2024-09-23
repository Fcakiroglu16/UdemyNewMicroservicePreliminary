using FluentValidation;

namespace UdemyMicroservices.Web.Pages.Auth.SignUp;

public class SignUpViewModelValidator : AbstractValidator<SignUpViewModel>
{
    public SignUpViewModelValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email Address is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty().WithMessage("Confirm Password is required.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.");
    }
}
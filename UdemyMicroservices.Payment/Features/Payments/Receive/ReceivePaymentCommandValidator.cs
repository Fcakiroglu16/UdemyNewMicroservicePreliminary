using FluentValidation;

namespace UdemyMicroservices.Payment.Features.Payments.Receive
{
    public class ReceivePaymentCommandValidator : AbstractValidator<ReceivePaymentCommand>
    {
        public ReceivePaymentCommandValidator()
        {
            // OrderCode - Must not be empty
            RuleFor(x => x.OrderCode)
                .NotEmpty().WithMessage("Order code is required.");

            // CardNumber - Must be 16 digits long
            RuleFor(x => x.CardNumber)
                .CreditCard().WithMessage("Card number is not valid.");

            // CardHolderName - Must not be empty and must be at least 3 characters long
            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("Card holder name is required.")
                .MinimumLength(3).WithMessage("Card holder name must be at least 3 characters long.");

            // ExpiryDate - Must be in MM/YY format and must be a valid future date
            RuleFor(x => x.ExpiryDate)
                .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$").WithMessage("Expiry date must be in MM/YY format.");

            // CVV - Must be exactly 3 or 4 digits
            RuleFor(x => x.CVV)
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits.");

            // Amount - Must be greater than zero
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}
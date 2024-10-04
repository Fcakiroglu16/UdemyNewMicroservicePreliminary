using FluentValidation;
using UdemyMicroservices.Web.Pages.Order.Dto;
using UdemyMicroservices.Web.Pages.Order.ViewModel;

namespace UdemyMicroservices.Web.Pages.Order.Validators
{
    public class CreateOrderViewModelValidator : AbstractValidator<CreateOrderViewModel>
    {
        public CreateOrderViewModelValidator()
        {
            RuleFor(x => x.Address)
                .NotNull().WithMessage("Address is required.")
                .SetValidator(new AddressViewModelValidator());

            RuleFor(x => x.Payment)
                .NotNull().WithMessage("Payment information is required.")
                .SetValidator(new PaymentViewModelValidator());
        }
    }

    public class AddressViewModelValidator : AbstractValidator<AddressViewModel>
    {
        public AddressViewModelValidator()
        {
            RuleFor(x => x.Line).NotEmpty().WithMessage("Address line is required.");
            RuleFor(x => x.Province).NotEmpty().WithMessage("Province is required.");
            RuleFor(x => x.District).NotEmpty().WithMessage("District is required.");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Zip code is required.");
        }
    }

    public class PaymentViewModelValidator : AbstractValidator<PaymentViewModel>
    {
        public PaymentViewModelValidator()
        {
            RuleFor(x => x.CardNumber).CreditCard().WithMessage("Invalid card number.");
            RuleFor(x => x.CardHolderName).NotEmpty().WithMessage("Card holder name is required.");
            RuleFor(x => x.ExpiryDate).NotEmpty().WithMessage("Expiry date is required.");
            RuleFor(x => x.Cvv).Matches(@"^\d{3,4}$").WithMessage("Invalid CVV.");
        }
    }
}
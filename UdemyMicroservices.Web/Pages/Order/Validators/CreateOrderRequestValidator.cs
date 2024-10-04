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
                .SetValidator(new AddressDtoValidator());

            RuleFor(x => x.Payment)
                .NotNull().WithMessage("Payment information is required.")
                .SetValidator(new PaymentDtoValidator());
        }
    }

    public class AddressDtoValidator : AbstractValidator<AddressViewModel>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.Line).NotEmpty().WithMessage("Address line is required.");
            RuleFor(x => x.Province).NotEmpty().WithMessage("Province is required.");
            RuleFor(x => x.District).NotEmpty().WithMessage("District is required.");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("Zip code is required.");
        }
    }

    public class PaymentDtoValidator : AbstractValidator<PaymentViewModel>
    {
        public PaymentDtoValidator()
        {
            RuleFor(x => x.CardNumber).CreditCard().WithMessage("Invalid card number.");
            RuleFor(x => x.CardHolderName).NotEmpty().WithMessage("Card holder name is required.");
            RuleFor(x => x.ExpiryDate).NotEmpty().WithMessage("Expiry date is required.");
            RuleFor(x => x.Cvv).Matches(@"^\d{3,4}$").WithMessage("Invalid CVV.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
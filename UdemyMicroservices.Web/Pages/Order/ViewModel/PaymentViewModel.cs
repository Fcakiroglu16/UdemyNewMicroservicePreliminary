using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Web.Pages.Order.ViewModel;

public record PaymentViewModel
{
    [Display(Name = "Card Number")] public string CardNumber { get; set; } = default!;

    [Display(Name = "Cardholder Name")] public string CardHolderName { get; set; } = default!;

    [Display(Name = "Expiry Date")] public string ExpiryDate { get; set; } = default!;

    [Display(Name = "CVV")] public string Cvv { get; set; } = default!;

    [Display(Name = "Payment Amount")] public decimal Amount { get; set; }

    public static PaymentViewModel Empty => new();
}
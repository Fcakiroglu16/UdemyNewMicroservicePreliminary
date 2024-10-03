namespace UdemyMicroservices.Web.Pages.Order.Dto
{
    public record PaymentDto(
        string CardNumber,
        string CardHolderName,
        string ExpiryDate,
        string Cvv,
        decimal Amount);
}
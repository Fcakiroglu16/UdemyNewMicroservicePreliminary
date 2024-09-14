namespace UdemyMicroservices.Payment.Features.Payments.GetAll
{
    public record PaymentsDto(string OrderCode, string Amount, string PaymentDate, string Status, string Error);
}
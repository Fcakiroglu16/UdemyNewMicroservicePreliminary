using UdemyMicroservices.Payment.Repositories;

namespace UdemyMicroservices.Payment.Features.Payments.Receive
{
    public record ReceivePaymentResponse(Guid PaymentId);
}
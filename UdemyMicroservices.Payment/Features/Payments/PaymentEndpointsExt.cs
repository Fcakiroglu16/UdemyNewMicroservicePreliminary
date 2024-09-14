using Asp.Versioning.Builder;
using UdemyMicroservices.Payment.Features.Payments.GetAll;
using UdemyMicroservices.Payment.Features.Payments.Receive;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Payment.Features.Payments;

public static class PaymentEndpointsExt
{
    public static void AddPaymentEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/payments")
            .MapReceivePaymentEndpoint()
            .MapGetAllPaymentsEndpoint()
            .WithTags("Payments").WithApiVersionSet(apiVersionSet);
    }
}
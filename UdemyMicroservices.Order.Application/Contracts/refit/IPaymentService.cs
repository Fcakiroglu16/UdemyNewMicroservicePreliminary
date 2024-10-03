using Refit;
using UdemyMicroservices.Order.Application.Features.Orders.CreateOrder;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Order.Application.Contracts.refit
{
    public interface IPaymentService
    {
        [Post("/api/v1/payments/receive")]
        Task<ApiResponse<ServiceResult<ReceivePaymentResponse>>> ReceivePaymentAsync(ReceivePaymentRequest request);
    }
}
using Refit;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Web.Pages.Order.Dto;

namespace UdemyMicroservices.Web.Services.Refit;

public interface IOrderService
{
    //CreateOrder endpoint
    [Post("/v1/order")]
    Task<ApiResponse<ServiceResult>> CreateOrder(CreateOrderRequest request);
}
using Refit;
using UdemyMicroservices.Web.Pages.Basket.Dto;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Services.Refit;

public interface IBasketService
{
    [Post("/v1/basket")]
    Task<ApiResponse<ServiceResult>> AddBasketAsync(AddBasketRequest request);


    [Post("/v1/basket/ApplyDiscountCoupon/{coupon}/{discountRate}")]
    Task<ApiResponse<ServiceResult>> ApplyDiscountRateAsync(string coupon, float discountRate);


    [Delete("/v1/basket/RemoveDiscountCoupon")]
    Task<ApiResponse<ServiceResult>> RemoveDiscountRateAsync();


    [Get("/v1/basket")]
    Task<ApiResponse<ServiceResult<BasketResponse>>> GetBasketsAsync();

    [Delete("/v1/basket/{courseId}")]
    Task<ApiResponse<ServiceResult>> DeleteItemAsync(Guid courseId);
}
using Refit;
using UdemyMicroservices.Web.Pages.Basket.Dto;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Services.Refit;

public interface IRefitDiscountService
{
    [Get("/v1/discount/{coupon}")]
    Task<ApiResponse<ServiceResult<GetDiscountByCouponResponse>>> GetDiscountByCoupon(string coupon);
}
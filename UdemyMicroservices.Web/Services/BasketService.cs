using UdemyMicroservices.Web.Extensions;
using UdemyMicroservices.Web.Pages.Basket.Dto;
using UdemyMicroservices.Web.Services.Refit;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Services
{
    public class BasketService(
        IBasketService basketService,
        IDiscountService discountService,
        ILogger<BasketService> logger)
    {
        public async Task<ServiceResult> CreateOrUpdateBasketAsync(AddBasketRequest request)
        {
            var responseAsResult = await basketService.AddBasketAsync(request);

            if (!responseAsResult.IsSuccessStatusCode)
            {
                logger.LogProblemDetails(responseAsResult.Error);
                return ServiceResult.Fail("An error occurred while creating or updating the basket");
            }


            return ServiceResult.Success();
        }


        public async Task<ServiceResult<BasketResponse>> GetBasketsAsync()
        {
            var responseAsResult = await basketService.GetBasketsAsync();

            if (!responseAsResult.IsSuccessStatusCode)
            {
                logger.LogProblemDetails(responseAsResult.Error);
                return ServiceResult<BasketResponse>.Fail("An error occurred while getting the baskets");
            }

            return responseAsResult.Content;
        }

        public async Task<ServiceResult> DeleteBasketAsync(Guid courseId)
        {
            var responseAsResult = await basketService.DeleteItemAsync(courseId);

            if (!responseAsResult.IsSuccessStatusCode)
            {
                logger.LogProblemDetails(responseAsResult.Error);
                return ServiceResult.Fail("An error occurred while deleting the basket");
            }

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> ApplyDiscountAsync(string coupon)
        {
            var responseAsResult = await discountService.GetDiscountByCoupon(coupon);

            if (!responseAsResult.IsSuccessStatusCode)
            {
                return ServiceResult.FailFromProblemDetails(responseAsResult.Error);
            }


            var discount = responseAsResult.Content.Data;

            var response = await basketService.ApplyDiscountRateAsync(coupon, discount.Rate);
            if (!responseAsResult.IsSuccessStatusCode)
            {
                logger.LogProblemDetails(responseAsResult.Error);
                return ServiceResult.Fail("An error occurred while applying the discount");
            }


            return ServiceResult.Success();
        }
    }
}
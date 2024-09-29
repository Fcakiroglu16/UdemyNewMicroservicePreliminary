using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Features.Basket.ApplyDiscountRate
{
    public class ApplyDiscountRateCommandHandler(IIdentityService identityService, IDistributedCache distributedCache)
        : IRequestHandler<ApplyDiscountRateCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(ApplyDiscountRateCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);
            var hasBasket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);


            if (string.IsNullOrEmpty(hasBasket))
                return ServiceResult.ErrorAsNotFound();

            var basket = JsonSerializer.Deserialize<BasketDto>(hasBasket);


            basket.ApplyNewDiscount(request.Coupon, request.Rate);

            await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
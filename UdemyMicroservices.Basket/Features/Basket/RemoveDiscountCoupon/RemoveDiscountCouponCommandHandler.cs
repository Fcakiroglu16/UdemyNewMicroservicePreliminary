using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Features.Basket.RemoveDiscountCoupon;

public record RemoveDiscountCouponCommand : IRequestByServiceResult;

public class RemoveDiscountCouponCommandHandler(
    IIdentityService identityService,
    IDistributedCache distributedCache) : IRequestHandler<RemoveDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(RemoveDiscountCouponCommand request,
        CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);
        var hasBasket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);


        if (string.IsNullOrEmpty(hasBasket))
            return ServiceResult.ErrorAsNotFound();

        var basket = JsonSerializer.Deserialize<BasketDto>(hasBasket);

        basket!.RemoveDiscount();


        await distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}
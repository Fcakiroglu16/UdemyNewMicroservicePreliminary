using System.Net;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Features.Basket.GetBasket;

public class GetAllBasketByUserIdQueryHandler(IDistributedCache distributedCache, IIdentityService identityService)
    : IRequestHandler<GetAllBasketByUserIdQuery, ServiceResult<BasketDto>>
{
    public async Task<ServiceResult<BasketDto>> Handle(GetAllBasketByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);
        var hasBasket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);


        if (string.IsNullOrEmpty(hasBasket))
            return ServiceResult<BasketDto>.SuccessAsOk(new BasketDto(identityService.GetUserId, []));

        var basket = JsonSerializer.Deserialize<BasketDto>(hasBasket);

        return ServiceResult<BasketDto>.SuccessAsOk(basket!);
    }
}
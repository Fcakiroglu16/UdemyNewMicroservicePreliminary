using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket;

public class SaveOrUpdateBasketCommandHandler(IDistributedCache distributedCache, IIdentityService identityService)
    : IRequestHandler<SaveOrUpdateBasketCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(SaveOrUpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var basketDto = new BasketDto(request.UserId, request.DiscountRate, request.BasketItems);


        var baskets = JsonSerializer.Serialize(basketDto);

        await distributedCache.SetStringAsync(string.Format(BasketConst.BasketCacheKey, identityService.GetUserId),
            baskets, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}
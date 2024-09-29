using System.Net;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Features.Basket.DeleteBasket;

public class DeleteBasketCommandHandler(IDistributedCache distributedCache, IIdentityService identityService)
    : IRequestHandler<DeleteBasketCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        //basket key
        var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);

        var hasBasket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);


        if (string.IsNullOrEmpty(hasBasket))
            return ServiceResult.Error("basket not found", "", HttpStatusCode.NotFound);


        var currentBasket = JsonSerializer.Deserialize<BasketDto>(hasBasket!);


        var basketItemToDelete = currentBasket!.BasketItems.FirstOrDefault(x => x.CourseId == request.CourseId);

        if (basketItemToDelete is null)
        {
            return ServiceResult.Error("basket Item not found", "", HttpStatusCode.NotFound);
        }


        currentBasket.BasketItems.Remove(basketItemToDelete!);


        if (currentBasket.BasketItems.Count == 1)
        {
            await distributedCache.RemoveAsync(cacheKey, cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }


        var basketAsJson = JsonSerializer.Serialize(currentBasket);

        await distributedCache.SetStringAsync(string.Format(BasketConst.BasketCacheKey, identityService.GetUserId),
            basketAsJson, cancellationToken);


        return ServiceResult.SuccessAsNoContent();
    }
}
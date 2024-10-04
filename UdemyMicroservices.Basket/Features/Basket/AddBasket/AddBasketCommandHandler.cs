using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Basket.Dto;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Features.Basket.AddBasket;

public class AddBasketCommandHandler(IDistributedCache distributedCache, IIdentityService identityService)
    : IRequestHandler<AddBasketCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = string.Format(BasketConst.BasketCacheKey, identityService.GetUserId);
        var hasBasket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

        BasketDto? currentBasket;

        var newBasketItem = new BasketItemDto(request.CourseId, request.CourseName, request.CoursePicture,
            request.CoursePrice, null);


        if (string.IsNullOrEmpty(hasBasket))
        {
            currentBasket = new BasketDto(identityService.GetUserId, [newBasketItem]);
        }

        else
        {
            currentBasket = JsonSerializer.Deserialize<BasketDto>(hasBasket);


            var existingBasketItem = currentBasket!.BasketItems.FirstOrDefault(x => x.Id == request.CourseId);


            if (existingBasketItem is not null)
            {
                currentBasket.BasketItems.Remove(existingBasketItem);
                currentBasket.BasketItems.Add(newBasketItem);
            }
            else
            {
                currentBasket.BasketItems.Add(newBasketItem);
            }
        }


        if (currentBasket.IsApplyDiscountRate) currentBasket.ApplyAvailableDiscount();


        var basketAsJson = JsonSerializer.Serialize(currentBasket);


        await distributedCache.SetStringAsync(cacheKey,
            basketAsJson, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}
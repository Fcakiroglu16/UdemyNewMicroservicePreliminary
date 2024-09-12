using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket
{
    public class SaveOrUpdateBasketCommandHandler(IDistributedCache distributedCache, IIdentityService identityService)
        : IRequestHandler<SaveOrUpdateBasketCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(SaveOrUpdateBasketCommand request, CancellationToken cancellationToken)
        {
            var baskets = JsonSerializer.Serialize(request.Basket);

            await distributedCache.SetStringAsync(string.Format(BasketConst.BasketCacheKey, identityService.GetUserId),
                baskets, token: cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
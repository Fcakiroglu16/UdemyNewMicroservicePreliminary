using MediatR;
using Microsoft.Extensions.Caching.Distributed;
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

        //delete basket
        await distributedCache.RemoveAsync(cacheKey, cancellationToken);


        return ServiceResult.SuccessAsNoContent();
    }
}
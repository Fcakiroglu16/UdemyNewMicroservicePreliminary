using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Basket.Features.Basket;
using UdemyMicroservices.Bus;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Consumers
{
    public class CreateOrderEventConsumer(IDistributedCache distributedCache)
        : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var cacheKey = string.Format(BasketConst.BasketCacheKey, context.Message.UserId);

            var hasBasket = await distributedCache.GetStringAsync(cacheKey);

            if (hasBasket is not null)
            {
                await distributedCache.RemoveAsync(cacheKey);
            }
        }
    }
}
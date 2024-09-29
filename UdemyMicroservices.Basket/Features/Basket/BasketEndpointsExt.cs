using Asp.Versioning.Builder;
using UdemyMicroservices.Basket.Features.Basket.AddBasket;
using UdemyMicroservices.Basket.Features.Basket.ApplyDiscountRate;
using UdemyMicroservices.Basket.Features.Basket.DeleteBasket;
using UdemyMicroservices.Basket.Features.Basket.GetBasket;

namespace UdemyMicroservices.Basket.Features.Basket;

public static class BasketEndpointsExt
{
    public static void AddBasketEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/baskets")
            .MapGetAllBasketByUserIdEndpoint()
            .MapAddBasketEndpoint()
            .MapDeleteBasketEndpoint()
            .MapApplyDiscountRateCommandEndpoint()
            .WithTags("baskets").WithApiVersionSet(apiVersionSet).RequireAuthorization();
    }
}
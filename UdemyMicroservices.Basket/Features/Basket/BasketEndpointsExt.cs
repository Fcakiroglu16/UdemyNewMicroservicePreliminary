using Asp.Versioning.Builder;
using UdemyMicroservices.Basket.Features.Basket.DeleteBasket;
using UdemyMicroservices.Basket.Features.Basket.GetBasket;
using UdemyMicroservices.Basket.Features.Basket.SaveOrUpdateBasket;

namespace UdemyMicroservices.Basket.Features.Basket;

public static class BasketEndpointsExt
{
    public static void AddBasketEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/baskets")
            .MapGetAllBasketByUserIdEndpoint()
            .MapSaveOrUpdateBasketEndpoint()
            .MapDeleteBasketEndpoint()
            .WithTags("baskets").WithApiVersionSet(apiVersionSet).RequireAuthorization();
    }
}
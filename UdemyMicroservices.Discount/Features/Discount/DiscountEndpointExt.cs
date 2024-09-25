using Asp.Versioning.Builder;
using UdemyMicroservices.Discount.Features.Discount.CreateDiscount;
using UdemyMicroservices.Discount.Features.Discount.GetDiscountByCode;

namespace UdemyMicroservices.Discount.Features.Discount;

public static class CourseEndpointsExt
{
    public static void AddDiscountEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/discounts")
            .MapCreateDiscountEndpoint()
            .MapDiscountByCodeEndpoint()
            .WithTags("discounts").WithApiVersionSet(apiVersionSet).RequireAuthorization();
    }
}
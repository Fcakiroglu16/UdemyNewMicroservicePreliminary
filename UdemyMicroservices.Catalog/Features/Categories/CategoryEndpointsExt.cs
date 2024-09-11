using Asp.Versioning.Builder;
using UdemyMicroservices.Catalog.Features.Categories.Create;
using UdemyMicroservices.Catalog.Features.Categories.GetAll;
using UdemyMicroservices.Catalog.Features.Categories.GetById;

namespace UdemyMicroservices.Catalog.Features.Categories;

public static class CategoryEndpointsExt
{
    public static void AddCategoryEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/categories")
            .MapCreateCategoryEndpoint()
            .MapCategoryByIdQueryEndpoint()
            .MapCategoryByIdQueryEndpointV2()
            .MapAllCategoryQueryEndpoint()
            .WithTags("Categories").WithApiVersionSet(apiVersionSet);
    }
}
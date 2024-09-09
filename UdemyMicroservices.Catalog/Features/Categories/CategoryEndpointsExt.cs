using UdemyMicroservices.Catalog.Features.Categories.Create;
using UdemyMicroservices.Catalog.Features.Categories.GetAll;
using UdemyMicroservices.Catalog.Features.Categories.GetById;

namespace UdemyMicroservices.Catalog.Features.Categories
{
    public static class CategoryEndpointsExt
    {
        public static void AddCategoryEndpointsExt(this WebApplication app)
        {
            app.MapCreateCategoryEndpoint();
            app.MapGetAllCategoryQueryEndpoint();
            app.MapGetByIdCategoryQueryEndpoint();
        }
    }
}
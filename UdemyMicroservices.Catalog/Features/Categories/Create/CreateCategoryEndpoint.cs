using MediatR;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.Create
{
    public static class CreateCategoryEndpoint
    {
        public static void MapCreateCategoryEndpoint(this WebApplication app)
        {
            app.MapPost("/categories",
                    async (CreateCategoryCommand request, IMediator mediator) =>
                        (await mediator.Send(request)).ToActionResult())
                .WithName("CreateCategory") // Endpoint'e isim verir
                .Produces<
                    CreateCategoryResponse>(StatusCodes
                    .Status201Created) // Üretilen yanıt türünü ve durum kodunu belirtir
                //.ProducesValidationProblem() // Doğrulama hatalarını belirtir
                //.RequireAuthorization() // Yetkilendirme gerektirir
                .WithTags("Categories"); // Endpoint'i belirli bir etiketle gruplar
        }
    }
}
using AutoMapper;
using MediatR;
using System.Net;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.GetById
{
    public record GetByIdCategoryQuery(string Id) : IRequestByServiceResult<CategoryDto>;

    public class GetByIdCategoryQueryHandler(IGenericRepository<Category> categoryRepository, IMapper mapper)
        : IRequestHandler<GetByIdCategoryQuery, ServiceResult<CategoryDto>>
    {
        public async Task<ServiceResult<CategoryDto>> Handle(GetByIdCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetById(request.Id);
            if (category is null)
            {
                return ServiceResult<CategoryDto>.Error("Category Not Found",
                    $"The category with id '{request.Id}' was not found.", HttpStatusCode.NotFound);
            }

            var categoryDto = mapper.Map<CategoryDto>(category);
            return ServiceResult<CategoryDto>.SuccessAsOk(categoryDto);
        }
    }

    public static class GetByIdCategoryQueryEndpoint
    {
        public static void MapGetByIdCategoryQueryEndpoint(this WebApplication app)
        {
            app.MapGet("/categories/{id}", async (IMediator mediator, string id) =>
                {
                    var response = await mediator.Send(new GetByIdCategoryQuery(id));
                    return Results.Ok(response);
                })
                .WithName("GetCategoryById")
                .Produces<CategoryDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Categories");
        }
    }
}
using AutoMapper;
using MediatR;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.GetAll
{
    public class GetAllCategoryQuery : IRequestByServiceResult<List<CategoryDto>>
    {
    }

    // handler
    public class GetAllCategoryQueryHandler(IGenericRepository<Category> categoryRepository, IMapper mapper)
        : IRequestHandler<GetAllCategoryQuery, ServiceResult<List<CategoryDto>>>
    {
        public async Task<ServiceResult<List<CategoryDto>>> Handle(GetAllCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAll();
            var categoryDtos = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.SuccessAsOk(categoryDtos);
        }
    }

    //endpoint
    public static class GetAllCategoryQueryEndpoint
    {
        public static void MapGetAllCategoryQueryEndpoint(this WebApplication app)
        {
            app.MapGet("/categories", async (IMediator mediator) =>
                {
                    var response = await mediator.Send(new GetAllCategoryQuery());
                    return Results.Ok(response);
                })
                .WithName("GetAllCategories")
                .Produces<List<CategoryDto>>(StatusCodes.Status200OK)
                .WithTags("Categories");
        }
    }
}
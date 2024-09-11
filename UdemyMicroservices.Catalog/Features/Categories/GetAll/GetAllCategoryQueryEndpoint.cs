using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.GetAll
{
    public class GetAllCategoryQuery : IRequestByServiceResult<List<CategoryDto>>
    {
    }

    // handler
    public class GetAllCategoryQueryHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<GetAllCategoryQuery, ServiceResult<List<CategoryDto>>>
    {
        public async Task<ServiceResult<List<CategoryDto>>> Handle(GetAllCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var categories = await context.Categories.ToListAsync(cancellationToken: cancellationToken);
            var categoryDtos = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.SuccessAsOk(categoryDtos);
        }
    }

    //endpoint
    public static class GetAllCategoryQueryEndpoint
    {
        public static void MapGetAllCategoryQueryEndpoint(this WebApplication app)
        {
            app.MapGet("/categories",
                    async (IMediator mediator) => (await mediator.Send(new GetAllCategoryQuery())).ToActionResult())
                .WithName("GetAllCategories")
                .Produces<List<CategoryDto>>(StatusCodes.Status200OK)
                .WithTags("Categories");
        }
    }
}
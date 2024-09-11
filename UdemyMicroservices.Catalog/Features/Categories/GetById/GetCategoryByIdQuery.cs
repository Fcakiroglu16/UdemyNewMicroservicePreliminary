using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.GetById
{
    public record GetCategoryByIdQuery(Guid Id) : IRequestByServiceResult<CategoryDto>;

    public class GetCategoryByIdQueryHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<GetCategoryByIdQuery, ServiceResult<CategoryDto>>
    {
        public async Task<ServiceResult<CategoryDto>> Handle(GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var category =
                await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id,
                    cancellationToken: cancellationToken);
            if (category is null)
            {
                return ServiceResult<CategoryDto>.Error("Category Not Found",
                    $"The category with id '{request.Id}' was not found.", HttpStatusCode.NotFound);
            }

            var categoryDto = mapper.Map<CategoryDto>(category);
            return ServiceResult<CategoryDto>.SuccessAsOk(categoryDto);
        }
    }

    public static class GetCategoryByIdQueryEndpoint
    {
        public static void MapGetByIdCategoryQueryEndpoint(this WebApplication app)
        {
            app.MapGet("/api/categories/{id:int}",
                    async (IMediator mediator, Guid id) =>
                        (await mediator.Send(new GetCategoryByIdQuery(id))).ToActionResult())
                .WithName("GetCategoryById")
                .Produces<CategoryDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Categories");
        }
    }
}
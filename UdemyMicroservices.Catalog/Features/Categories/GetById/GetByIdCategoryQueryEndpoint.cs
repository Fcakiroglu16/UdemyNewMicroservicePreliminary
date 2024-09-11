using AutoMapper;
using MediatR;
using System.Net;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.GetById
{
    public record GetByIdCategoryQuery(Guid Id) : IRequestByServiceResult<CategoryDto>;

    public class GetByIdCategoryQueryHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<GetByIdCategoryQuery, ServiceResult<CategoryDto>>
    {
        public async Task<ServiceResult<CategoryDto>> Handle(GetByIdCategoryQuery request,
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

    public static class GetByIdCategoryQueryEndpoint
    {
        public static void MapGetByIdCategoryQueryEndpoint(this WebApplication app)
        {
            app.MapGet("/categories/{id:int}",
                    async (IMediator mediator, Guid id) =>
                        (await mediator.Send(new GetByIdCategoryQuery(id))).ToActionResult())
                .WithName("GetCategoryById")
                .Produces<CategoryDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Categories");
        }
    }
}
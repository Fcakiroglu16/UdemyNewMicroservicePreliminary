using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Categories.GetById;

public record GetCategoryByIdQuery(Guid Id) : IRequestByServiceResult<CategoryDto>;

public class GetCategoryByIdQueryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetCategoryByIdQuery, ServiceResult<CategoryDto>>
{
    public async Task<ServiceResult<CategoryDto>> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category =
            await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id,
                cancellationToken);
        if (category is null)
            return ServiceResult<CategoryDto>.Error("Category Not Found",
                $"The category with id '{request.Id}' was not found.", HttpStatusCode.NotFound);

        var categoryDto = mapper.Map<CategoryDto>(category);
        return ServiceResult<CategoryDto>.SuccessAsOk(categoryDto);
    }
}

public static class GetCategoryByIdQueryEndpoint
{
    public static RouteGroupBuilder MapCategoryByIdQueryEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}",
                async (IMediator mediator, Guid id) =>
                    (await mediator.Send(new GetCategoryByIdQuery(id))).ToActionResult())
            .WithName("GetCategoryById")
            .Produces<CategoryDto>()
            .Produces(StatusCodes.Status404NotFound).MapToApiVersion(1.0);
        return group;
    }


    public static RouteGroupBuilder MapCategoryByIdQueryEndpointV2(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}",
                (IMediator mediator, Guid id) =>
                {
                    var result =
                        ServiceResult<CategoryDto>.SuccessAsOk(new CategoryDto(id.ToString(),
                            "Category V2"));


                    return result.ToActionResult();
                })
            .WithName("GetCategoryByIdV2")
            .Produces<CategoryDto>()
            .Produces(StatusCodes.Status404NotFound).MapToApiVersion(2.0);
        return group;
    }
}
using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Courses.GetById;

public record GetCourseByIdQuery(Guid Id) : IRequest<ServiceResult<CourseDto>>;

public class GetCourseByIdCourseQueryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetCourseByIdQuery, ServiceResult<CourseDto>>
{
    public async Task<ServiceResult<CourseDto>> Handle(GetCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course is null)
            return ServiceResult<CourseDto>.Error("Course Not Found",
                $"The course with id '{request.Id}' was not found.", HttpStatusCode.NotFound);

        var category =
            await context.Categories.FirstOrDefaultAsync(x => x.Id == course.CategoryId, cancellationToken);
        course.Category = category;

        var courseDto = mapper.Map<CourseDto>(course);
        return ServiceResult<CourseDto>.SuccessAsOk(courseDto);
    }
}

public static class GetCourseByIdQueryEndpoint
{
    public static RouteGroupBuilder MapCourseByIdQueryEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (IMediator mediator, Guid id) =>
            {
                var response = await mediator.Send(new GetCourseByIdQuery(id));
                return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
            })
            .WithName("GetCourseById")
            .Produces<CourseDto>()
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }
}
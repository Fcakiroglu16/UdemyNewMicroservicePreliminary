using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Courses.GetById
{
    public record GetByIdCourseQuery(Guid Id) : IRequest<ServiceResult<CourseDto>>;

    public class GetByIdCourseQueryHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<GetByIdCourseQuery, ServiceResult<CourseDto>>
    {
        public async Task<ServiceResult<CourseDto>> Handle(GetByIdCourseQuery request,
            CancellationToken cancellationToken)
        {
            var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (course is null)
            {
                return ServiceResult<CourseDto>.Error("Course Not Found",
                    $"The course with id '{request.Id}' was not found.", HttpStatusCode.NotFound);
            }

            var category =
                await context.Categories.FirstOrDefaultAsync(x => x.Id == course.CategoryId, cancellationToken);
            course.Category = category;

            var courseDto = mapper.Map<CourseDto>(course);
            return ServiceResult<CourseDto>.SuccessAsOk(courseDto);
        }
    }

    public static class GetByIdCourseQueryEndpoint
    {
        public static void MapGetByIdCourseQueryEndpoint(this WebApplication app)
        {
            app.MapGet("/courses/{id}", async (IMediator mediator, Guid id) =>
                {
                    var response = await mediator.Send(new GetByIdCourseQuery(id));
                    return response.IsSuccess ? Results.Ok(response) : Results.NotFound(response);
                })
                .WithName("GetCourseById")
                .Produces<CourseDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Courses");
        }
    }
}
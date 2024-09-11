using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Courses.GetAllByUserId
{
    public record GetAllCourseByUserIdQuery(string UserId) : IRequest<ServiceResult<List<CourseDto>>>;

    public class GetCourseByUserIdQueryHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<GetAllCourseByUserIdQuery, ServiceResult<List<CourseDto>>>
    {
        public async Task<ServiceResult<List<CourseDto>>> Handle(GetAllCourseByUserIdQuery request,
            CancellationToken cancellationToken)
        {
            var courses = await context.Courses.Where(x => x.UserId == request.UserId)
                .ToListAsync(cancellationToken: cancellationToken);


            if (!courses.Any())
            {
                return ServiceResult<List<CourseDto>>.Error("Courses Not Found",
                    $"The courses with user id '{request.UserId}' were not found.", HttpStatusCode.NotFound);
            }


            var categories = await context.Categories.ToListAsync(cancellationToken: cancellationToken);


            foreach (var course in courses)
            {
                course.Category = categories.First(x => x.Id == course.CategoryId);
            }


            var coursesListDtos = mapper.Map<List<CourseDto>>(courses);


            return ServiceResult<List<CourseDto>>.SuccessAsOk(coursesListDtos);
        }
    }

    public static class GetAllCourseByUserIdEndpoint
    {
        public static void MapGetCourseByUserIdEndpoint(this WebApplication app)
        {
            app.MapGet("/api/courses/user/{userId}",
                    async (IMediator mediator, string userId) =>
                        (await mediator.Send(new GetAllCourseByUserIdQuery(userId))).ToActionResult())
                .WithName("GetAllCourseByUserId")
                .Produces<List<CourseDto>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags("Courses");
        }
    }
}
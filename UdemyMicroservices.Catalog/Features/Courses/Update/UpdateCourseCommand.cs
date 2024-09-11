using AutoMapper;
using MediatR;
using System.Net;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Courses.Update
{
    public record UpdateCourseCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string Picture,
        string UserId,
        Guid CategoryId,
        int Duration) : IRequest<ServiceResult>;

    public class UpdateCourseCommandHandler(AppDbContext context, IMapper mapper)
        : IRequestHandler<UpdateCourseCommand, ServiceResult>
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ServiceResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (course == null)
            {
                return ServiceResult.Error("Course Not Found", $"The course with id '{request.Id}' was not found.",
                    HttpStatusCode.NotFound);
            }

            var category =
                await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (category == null)
            {
                return ServiceResult.Error("Category Not Found",
                    $"The category with id '{request.CategoryId}' was not found.", HttpStatusCode.NotFound);
            }

            course.Name = request.Name;
            course.Description = request.Description;
            course.Price = request.Price;
            course.Picture = request.Picture;
            course.UserId = request.UserId;
            course.CategoryId = request.CategoryId;


            course.Feature ??= new Feature();

            course.Feature.Duration = request.Duration;


            await _context.SaveChangesAsync(cancellationToken);

            var courseDto = _mapper.Map<CourseDto>(course);
            return ServiceResult.SuccessAsNoContent();
        }
    }

    public static class UpdateCourseCommandEndpoint
    {
        public static void MapUpdateCourseCommandEndpoint(this WebApplication app)
        {
            app.MapPut("/api/courses",
                    async (IMediator mediator, UpdateCourseCommand command) =>
                    {
                        var response = await mediator.Send(command);
                        return response.ToActionResult();
                    })
                .WithName("UpdateCourse")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithTags("Courses");
        }
    }
}
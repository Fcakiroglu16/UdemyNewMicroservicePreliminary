using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Courses.Create;

public record CreateCourseCommand(
    string Name,
    string Description,
    decimal Price,
    string Picture,
    string UserId,
    Guid CategoryId,
    int Duration) : IRequestByServiceResult<CourseDto>;

public class CreateCourseCommandHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<CreateCourseCommand, ServiceResult<CourseDto>>
{
    public async Task<ServiceResult<CourseDto>> Handle(CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var category =
            await context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null)
            return ServiceResult<CourseDto>.Error("Category Not Found",
                $"The category with id '{request.CategoryId}' was not found.", HttpStatusCode.NotFound);

        // course name is same as another course name
        var courseName = await context.Courses.FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);
        if (courseName != null)
            return ServiceResult<CourseDto>.Error("Course Name Already Exists",
                $"The course name '{request.Name}' already exists.", HttpStatusCode.BadRequest);

        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Picture = request.Picture,
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            CreatedTime = DateTime.UtcNow,
            Feature = new Feature { Duration = request.Duration }
        };

        context.Courses.Add(course);
        await context.SaveChangesAsync(cancellationToken);

        var courseDto = mapper.Map<CourseDto>(course);
        return ServiceResult<CourseDto>.SuccessAsCreated(courseDto, "");
    }
}

public static class CreateCourseCommandEndpoint
{
    public static RouteGroupBuilder MapCreateCourseCommandEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IMediator mediator, CreateCourseCommand command) =>
                    (await mediator.Send(command)).ToActionResult())
            .WithName("CreateCourse")
            .Produces<CourseDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<CreateCourseCommand>>();


        return group;
    }
}
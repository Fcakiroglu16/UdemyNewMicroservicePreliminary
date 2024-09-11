using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Catalog.Features.Courses.Delete;

public record DeleteCourseCommand(Guid Id) : IRequest<ServiceResult>;

public class DeleteCourseCommandHandler(AppDbContext context) : IRequestHandler<DeleteCourseCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await context.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course == null)
            return ServiceResult.Error("Course Not Found", $"The course with id '{request.Id}' was not found.",
                HttpStatusCode.NotFound);

        context.Courses.Remove(course);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public static class DeleteCourseCommandEndpoint
{
    public static void MapDeleteCourseCommandEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/courses/{id}",
                async (IMediator mediator, Guid id) =>
                {
                    var response = await mediator.Send(new DeleteCourseCommand(id));
                    return response.ToActionResult();
                })
            .WithName("DeleteCourse")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Courses");
    }
}
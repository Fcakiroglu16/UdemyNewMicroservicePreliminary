using MediatR;
using Microsoft.Extensions.FileProviders;
using System.Net;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Files.Features.File.Delete
{
    public class DeleteFileCommandHandler(IFileProvider fileProvider)
        : IRequestHandler<DeleteFileCommand, ServiceResult>
    {
        public Task<ServiceResult> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var fileInfo = fileProvider.GetFileInfo(Path.Combine("uploads", request.FileName));
            if (!fileInfo.Exists)
            {
                return Task.FromResult(ServiceResult.Error("File Not Found", "The specified file does not exist.",
                    HttpStatusCode.NotFound));
            }


            System.IO.File.Delete(fileInfo.PhysicalPath!);
            return Task.FromResult(ServiceResult.SuccessAsNoContent());
        }
    }

    public static class DeleteFileEndpoint
    {
        public static RouteGroupBuilder MapDeleteFileEndpoint(this RouteGroupBuilder group)
        {
            group.MapDelete("/{fileName:required}",
                    async (IMediator mediator, string fileName) =>
                    {
                        var result = await mediator.Send(new DeleteFileCommand(fileName));
                        return result.ToActionResult();
                    })
                .WithName("DeleteFile")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .MapToApiVersion(1.0);

            return group;
        }
    }
}
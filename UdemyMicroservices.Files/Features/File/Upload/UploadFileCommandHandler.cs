using System.Net;
using MediatR;
using Microsoft.Extensions.FileProviders;
using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Files.Features.File.Upload
{
    public record UploadFileCommand(IFormFile File) : IRequestByServiceResult<UploadFileCommandResponse>;

    public record UploadFileCommandResponse(
        string FileName,
        string FilePath,
        string OriginalFileName,
        HttpStatusCode StatusCode);

    public class UploadFileCommandHandler(IFileProvider fileProvider)
        : IRequestHandler<UploadFileCommand, ServiceResult<UploadFileCommandResponse>>
    {
        public async Task<ServiceResult<UploadFileCommandResponse>> Handle(UploadFileCommand request,
            CancellationToken cancellationToken)
        {
            if (request.File?.Length == 0)
            {
                return ServiceResult<UploadFileCommandResponse>.Error("Invalid File",
                    "The provided file is empty or null.",
                    HttpStatusCode.BadRequest);
            }


            string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}";
            string uploadPath = Path.Combine(fileProvider.GetFileInfo("uploads").PhysicalPath!, newFileName);


            await using var stream = new FileStream(uploadPath, FileMode.Create);
            await request.File.CopyToAsync(stream, cancellationToken);


            return ServiceResult<UploadFileCommandResponse>.SuccessAsOk(new UploadFileCommandResponse(
                FileName: newFileName,
                FilePath: $"uploads/{newFileName}",
                OriginalFileName: request.File.FileName,
                StatusCode: HttpStatusCode.OK));
        }
    }

    public static class UploadFileEndpoint
    {
        public static RouteGroupBuilder MapUploadFileEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/",
                    async (IMediator mediator, IFormFile file) =>
                    {
                        var result = await mediator.Send(new UploadFileCommand(file));
                        return result.ToActionResult();
                    })
                .WithName("UploadFile")
                .Produces(StatusCodes.Status200OK)
                .MapToApiVersion(1.0).DisableAntiforgery();

            return group;
        }
    }
}
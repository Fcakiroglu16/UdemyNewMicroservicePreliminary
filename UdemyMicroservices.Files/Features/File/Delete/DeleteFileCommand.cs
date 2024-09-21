using UdemyMicroservices.Shared;

namespace UdemyMicroservices.Files.Features.File.Delete
{
    public record DeleteFileCommand(string FileName) : IRequestByServiceResult;
}
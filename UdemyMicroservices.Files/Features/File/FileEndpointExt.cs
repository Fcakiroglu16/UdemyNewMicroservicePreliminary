using Asp.Versioning.Builder;
using UdemyMicroservices.Files.Features.File.Delete;
using UdemyMicroservices.Files.Features.File.Upload;

namespace UdemyMicroservices.Files.Features.File;

public static class CourseEndpointsExt
{
    public static void AddFileEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/files")
            .MapUploadFileEndpoint()
            .MapDeleteFileEndpoint()
            .WithTags("files").WithApiVersionSet(apiVersionSet);
    }
}
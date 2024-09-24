using Refit;
using UdemyMicroservices.Web.Pages.Instructor.Course.Dto;
using UdemyMicroservices.Web.Shared;

namespace UdemyMicroservices.Web.Services
{
    public interface IFileService
    {
        [Multipart]
        [Post("/v1/file")]
        Task<ServiceResult<UploadFileResponse>> UploadFile([AliasAs("file")] StreamPart file);
    }
}
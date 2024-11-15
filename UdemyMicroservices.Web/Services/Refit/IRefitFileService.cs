﻿using Refit;
using UdemyMicroservices.Web.Pages.Instructor.Course.Dto;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Services.Refit;

public interface IRefitFileService
{
    [Multipart]
    [Post("/v1/file")]
    Task<ServiceResult<UploadFileResponse>> UploadFileAsync([AliasAs("file")] StreamPart file);


    [Delete("/v1/file")]
    Task<ApiResponse<ServiceResult>> DeleteFileAsync([Body] DeleteFileRequest request);
}
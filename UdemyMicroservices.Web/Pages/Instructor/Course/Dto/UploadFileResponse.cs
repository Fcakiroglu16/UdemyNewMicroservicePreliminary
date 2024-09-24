namespace UdemyMicroservices.Web.Pages.Instructor.Course.Dto
{
    public record UploadFileResponse(
        string FileName,
        string FilePath,
        string OriginalFileName);
}
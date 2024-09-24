using Refit;
using UdemyMicroservices.Web.Pages.Instructor.Course.Dto;
using UdemyMicroservices.Web.Pages.Instructor.CreateCourse;
using UdemyMicroservices.Web.Shared;

namespace UdemyMicroservices.Web.Services
{
    public interface ICatalogService
    {
        [Post("/v1/catalog/courses")]
        Task<ApiResponse<ServiceResult>> CreateCourseAsync(CreateCourseRequest request);

        [Get("/v1/catalog/categories")]
        Task<ApiResponse<ServiceResult<List<CategoryResponse>>>> GetCategoriesAsync();


        [Get("/v1/catalog/courses")]
        Task<ApiResponse<ServiceResult<List<CourseResponse>>>> GetAllCourses();
    }
}
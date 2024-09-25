using Refit;
using UdemyMicroservices.Web.Pages.Instructor.Course.Dto;
using UdemyMicroservices.Web.Pages.Instructor.CreateCourse;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Services.Refit;

public interface ICatalogService
{
    [Post("/v1/catalog/courses")]
    Task<ApiResponse<ServiceResult>> CreateCourseAsync(CreateCourseRequest request);


    [Put("/v1/catalog/courses")]
    Task<ApiResponse<ServiceResult>> UpdateCourseAsync(UpdateCourseRequest request);


    [Get("/v1/catalog/categories")]
    Task<ApiResponse<ServiceResult<List<CategoryResponse>>>> GetCategoriesAsync();


    [Get("/v1/catalog/courses")]
    Task<ApiResponse<ServiceResult<List<CourseResponse>>>> GetAllCourses();

    [Get("/v1/catalog/courses/user/{userId}")]
    Task<ApiResponse<ServiceResult<List<CourseResponse>>>> GetAllCoursesByUserId(Guid userId);

    [Delete("/v1/catalog/courses/{id}")]
    Task<ApiResponse<ServiceResult>> DeleteCourseAsync(Guid id);


    [Get("/v1/catalog/courses/{id}")]
    Task<ApiResponse<ServiceResult<CourseResponse>>> GetCourse(Guid id);
}
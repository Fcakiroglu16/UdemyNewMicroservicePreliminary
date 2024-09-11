using Asp.Versioning.Builder;
using UdemyMicroservices.Catalog.Features.Courses.Create;
using UdemyMicroservices.Catalog.Features.Courses.Delete;
using UdemyMicroservices.Catalog.Features.Courses.GetAll.UdemyMicroservices.Catalog.Features.Courses.GetAll;
using UdemyMicroservices.Catalog.Features.Courses.GetAllByUserId;
using UdemyMicroservices.Catalog.Features.Courses.GetById;
using UdemyMicroservices.Catalog.Features.Courses.Update;

namespace UdemyMicroservices.Catalog.Features.Courses;

public static class CourseEndpointsExt
{
    public static void AddCourseEndpointsExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/courses")
            .MapAllCoursesQueryEndpoint()
            .MapAllCourseByUserIdEndpoint()
            .MapCourseByIdQueryEndpoint()
            .MapCreateCourseCommandEndpoint()
            .MapUpdateCourseCommandEndpoint()
            .MapDeleteCourseCommandEndpoint()
            .WithTags("Courses").WithApiVersionSet(apiVersionSet);
    }
}
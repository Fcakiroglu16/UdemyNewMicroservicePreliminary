using UdemyMicroservices.Catalog.Features.Courses.GetAll.UdemyMicroservices.Catalog.Features.Courses.GetAll;
using UdemyMicroservices.Catalog.Features.Courses.GetById;

namespace UdemyMicroservices.Catalog.Features.Courses
{
    public static class CourseEndpointsExt
    {
        public static void AddCourseEndpointsExt(this WebApplication app)
        {
            app.MapGetAllCoursesQueryEndpoint();
            app.MapGetByIdCourseQueryEndpoint();
        }
    }
}
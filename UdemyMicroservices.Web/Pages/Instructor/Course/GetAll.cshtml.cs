using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.Shared;

namespace UdemyMicroservices.Web.Pages.Instructor.Course
{
    public class GetAllCoursesModel(CatalogService catalogService) : PageModel
    {
        public List<CourseViewModel>? CourseList { get; set; }

        public async Task OnGet()
        {
            var response = await catalogService.GetAllCourses();


            if (response.IsFail)
            {
                ViewData["error"] = new PageErrorModel(response.ProblemDetails!.Title, response.ProblemDetails.Detail);
                return;
            }

            CourseList = response.Data!;
        }
    }
}
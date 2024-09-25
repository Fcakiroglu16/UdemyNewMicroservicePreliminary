using Microsoft.AspNetCore.Mvc.RazorPages;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Instructor;

public class IndexModel(CatalogService catalogService) : PageModel
{
    public CourseStatisticsViewModel? CourseStatisticsViewModel { get; set; }

    public async Task OnGet()
    {
        var result = await catalogService.GetCourseStatistic();

        if (result.IsFail)
        {
            ViewData["Error"] = new PageErrorModel(result.ProblemDetails!.Title, result.ProblemDetails.Detail);


            return;
        }


        CourseStatisticsViewModel = new CourseStatisticsViewModel
        {
            CourseCount = result.Data!.CourseCount,
            AverageRating = result.Data!.AverageRating
        };
    }
}
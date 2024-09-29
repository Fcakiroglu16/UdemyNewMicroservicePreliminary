using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages;

public class IndexModel(CatalogService catalogService) : BasePageModel
{
    public List<CourseViewModel>? Courses { get; set; } = new();

    public async Task<IActionResult> OnGet()
    {
        var coursesAsResult = await catalogService.GetAllCoursesAsync();

        if (coursesAsResult.IsFail) return ErrorPage(coursesAsResult);

        Courses = coursesAsResult.Data!;

        return Page();
    }
}
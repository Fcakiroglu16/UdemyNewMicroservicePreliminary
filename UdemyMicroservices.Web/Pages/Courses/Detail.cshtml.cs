using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Courses;

public class DetailModel(CatalogService catalogService) : BasePageModel
{
    public CourseViewModel? Course { get; set; }

    public async Task<IActionResult> OnGet(Guid id)
    {
        var courseAsResult = await catalogService.GetCourse(id);

        if (courseAsResult.IsFail) return ErrorPage(courseAsResult);

        Course = courseAsResult.Data!;
        return Page();
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Constants;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Instructor.Course;

[Authorize(Roles = RoleConstants.Instructor)]
public class GetAllCoursesModel(CatalogService catalogService) : BasePageModel
{
    public List<CourseViewModel>? CourseList { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var result = await catalogService.GetAllCoursesByUserId();

        if (result.IsFail) return ErrorPage(result);

        CourseList = result.Data!;
        return Page();
    }

    public async Task<IActionResult> OnGetDeleteAsync(Guid id)
    {
        var result = await catalogService.DeleteCourseAsync(id);

        return result.IsFail ? ErrorPage(result) : RedirectToPage("/Instructor/Course/GetAll");
    }
}
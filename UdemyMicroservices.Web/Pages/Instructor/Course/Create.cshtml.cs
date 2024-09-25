using Microsoft.AspNetCore.Mvc;
using UdemyMicroservices.Web.Pages.Instructor.Course.ViewModel;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Instructor.Course;

//[Authorize(Roles = RoleConstants.Instructor)]
public class CreateCourseModel(CatalogService catalogService) : BasePageModel
{
    [BindProperty] public CreateCourseViewModel ViewModel { get; set; } = CreateCourseViewModel.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await catalogService.GetCategoryList();

        if (result.IsFail) return ErrorPage(result);

        ViewModel.SetCategoryDropdownList(result.Data!);
        return Page();
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var categoriesAsResult = await catalogService.GetCategoryList();
            if (categoriesAsResult.IsFail) return ErrorPage(categoriesAsResult);
            ViewModel.SetCategoryDropdownList(categoriesAsResult.Data!);
            return Page();
        }

        var courseToCreateResult = await catalogService.CreateCourseAsync(ViewModel);
        return courseToCreateResult.IsFail ? ErrorPage(courseToCreateResult) : RedirectToPage("GetAll");
    }
}
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdemyMicroservices.Web.Extensions;
using UdemyMicroservices.Web.Pages.Instructor.CreateCourse;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UdemyMicroservices.Web.Pages.Instructor.Course
{
    //[Authorize(Roles = RoleConstants.Instructor)]
    public class CreateCourseModel(CatalogService catalogService) : BasePageModel
    {
        [BindProperty]
        public ViewModel.CreateCourseViewModel ViewModel { get; set; } = Course.ViewModel.CreateCourseViewModel.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var result = await catalogService.GetCategoryList();

            if (result.IsFail) return ErrorResultPage(result);

            ViewModel.SetCategoryDropdownList(result.Data!);
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categoriesAsResult = await catalogService.GetCategoryList();
                if (categoriesAsResult.IsFail) return ErrorResultPage(categoriesAsResult);
                ViewModel.SetCategoryDropdownList(categoriesAsResult.Data!);
                return Page();
            }

            var courseToCreateResult = await catalogService.CreateCourseAsync(ViewModel);
            return courseToCreateResult.IsFail ? ErrorResultPage(courseToCreateResult) : RedirectToPage("GetAll");
        }
    }
}
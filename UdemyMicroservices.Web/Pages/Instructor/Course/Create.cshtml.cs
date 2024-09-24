using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdemyMicroservices.Web.Pages.Instructor.CreateCourse;
using UdemyMicroservices.Web.Services;

namespace UdemyMicroservices.Web.Pages.Instructor.Course
{
    //[Authorize(Roles = RoleConstants.Instructor)]
    public class CreateCourseModel(CatalogService catalogService) : PageModel
    {
        [BindProperty]
        public ViewModel.CreateCourseModel ViewModel { get; set; } = Course.ViewModel.CreateCourseModel.Empty;

        public async Task OnGetAsync()
        {
            var categoryListAsResult = await catalogService.GetCategoryList();

            if (categoryListAsResult.IsFail)
            {
                // ModelState.AddModelError(string.Empty, categoryListAsResult.Error!);
            }

            ViewModel.CategoryDropdownList = new SelectList(categoryListAsResult.Data!, "Id", "Name");
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var result = await catalogService.CreateCourseAsync(ViewModel);


            if (result.IsFail)
            {
                // ModelState.AddModelError(string.Empty, result.Error!);
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}
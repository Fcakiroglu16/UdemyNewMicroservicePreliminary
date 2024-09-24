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
    public class CreateCourseModel(CatalogService catalogService) : PageModel
    {
        [BindProperty]
        public ViewModel.CreateCourseViewModel ViewModel { get; set; } = Course.ViewModel.CreateCourseViewModel.Empty;

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
            if (!ModelState.IsValid)
            {
                var categoryListAsResult = await catalogService.GetCategoryList();
                ViewModel.CategoryDropdownList = new SelectList(categoryListAsResult.Data!, "Id", "Name");
                return Page();
            }


            var result = await catalogService.CreateCourseAsync(ViewModel);


            if (!result.IsFail) return RedirectToPage("GetAll");


            if (result.ProblemDetails is null) return Page();


            ViewData["Error"] = new PageErrorModel(result.ProblemDetails.Title, result.ProblemDetails.Detail);

            var validationError = result.ProblemDetails.Extensions.FirstOrDefault(x => x.Key == "errors");


            if (validationError.Value is null) return Page();

            var validationErrorAsDictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(
                validationError.Value!
                    .ToString()!);

            foreach (var fieldError in validationErrorAsDictionary!.SelectMany(fieldErrors => fieldErrors.Value))
            {
                ModelState.AddModelError(string.Empty, fieldError);
            }


            return Page();
        }
    }
}
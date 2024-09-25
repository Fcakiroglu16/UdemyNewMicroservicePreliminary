using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UdemyMicroservices.Web.ViewModels
{
    public class BasePageModel : PageModel
    {
        public IActionResult ErrorResultPage(ServiceResult serviceResult)
        {
            ViewData["Error"] =
                new PageErrorModel(serviceResult.ProblemDetails!.Title, serviceResult.ProblemDetails.Detail);


            var validationError = serviceResult.ProblemDetails.Extensions.FirstOrDefault(x => x.Key == "errors");

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
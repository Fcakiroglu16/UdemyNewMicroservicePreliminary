using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UdemyMicroservices.Web.Pages.Auth.SignUp;

namespace UdemyMicroservices.Web.Pages.Auth;

public class SignUpModel(SignUpService signUpService) : PageModel
{
    [BindProperty] public SignUpViewModel SignUpViewModel { get; set; } = SignUpViewModel.GetExampleModel;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();


        var result = await signUpService.SignUpAsync(SignUpViewModel);


        if (result.IsFail)
        {
            //ModelState.AddModelError(string.Empty, result.Error!);
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UdemyMicroservices.Web.Pages.Auth.SignUp;

public class SignUpModel(AuthService authService) : PageModel
{
    public SignUpViewModel SignUpViewModel { get; set; } = SignUpViewModel.GetExampleModel;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await authService.SignUpAsync(SignUpViewModel);

        if (result.IsFail)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UdemyMicroservices.Web.Pages.Auth.SignIn;

namespace UdemyMicroservices.Web.Pages.Auth;

public class SignInModel(SignInService signInService) : PageModel
{
    [BindProperty] public SignInViewModel SignInViewModel { get; set; } = SignInViewModel.ExampleModel;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();


        var result = await signInService.SignInAsync(SignInViewModel);

        if (result.IsFail)
            // ModelState.AddModelError(string.Empty, result.Error);
            return Page();

        return RedirectToPage("/Index");
    }


    public async Task<IActionResult> OnGetSignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Index");
    }
}
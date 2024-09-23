using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Web.Pages.Auth.SignUp;

public record SignUpViewModel(
    [Display(Name = "First Name")] string FirstName,
    [Display(Name = "Last Name")] string LastName,
    [Display(Name = "Username")] string UserName,
    [Display(Name = "Email Address")] string Email,
    [Display(Name = "Password")] string Password,
    [Display(Name = "Confirm Password")] string PasswordConfirm)
{
    public static SignUpViewModel Empty => new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
        string.Empty);

    public static SignUpViewModel GetExampleModel => new("Ahmet", "Yıldız", "ahmet16", "ahmet@outlook.com",
        "Password12*",
        "Password12*");
}
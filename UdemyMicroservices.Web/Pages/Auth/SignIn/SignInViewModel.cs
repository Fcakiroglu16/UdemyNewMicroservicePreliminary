namespace UdemyMicroservices.Web.Pages.Auth.SignIn
{
    public record SignInViewModel(string Email, string Password, bool RememberMe)
    {
        public static SignInViewModel Empty => new(string.Empty, string.Empty, false);

        public static SignInViewModel ExampleModel => new("ahmet@outlook.com", "Password12*", true);
    }
}
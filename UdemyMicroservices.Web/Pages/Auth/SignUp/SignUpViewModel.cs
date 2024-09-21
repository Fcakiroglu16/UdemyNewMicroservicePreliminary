namespace UdemyMicroservices.Web.Pages.Auth.SignUp
{
    public record SignUpViewModel(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password,
        string PasswordConfirm)
    {
        public static SignUpViewModel Empty => new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            string.Empty);

        public static SignUpViewModel GetExampleModel => new("Ahmet", "Yıldız", "ahmet16", "ahmet@outlook.com",
            "Password12*",
            "Password12*");
    }
}
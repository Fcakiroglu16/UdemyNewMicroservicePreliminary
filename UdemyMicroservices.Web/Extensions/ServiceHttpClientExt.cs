using UdemyMicroservices.Web.Pages.Auth.SignIn;
using UdemyMicroservices.Web.Pages.Auth.SignUp;
using UdemyMicroservices.Web.Services;

namespace UdemyMicroservices.Web.Extensions;

public static class ServiceHttpClientExt
{
    public static void AddHttpClientsExt(this IServiceCollection services)
    {
        services.AddHttpClient<SignUpService>();
        services.AddHttpClient<SignInService>();
        services.AddHttpClient<TokenService>();
    }
}
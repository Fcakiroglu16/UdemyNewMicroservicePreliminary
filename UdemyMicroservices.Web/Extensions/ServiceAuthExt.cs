using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

namespace UdemyMicroservices.Web.Extensions;

public static class ServiceAuthExt
{
    public static IServiceCollection AddAuthenticationExt(this IServiceCollection services)
    {
        //PersistKeysToFileSystem
        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Keys")))
            .SetDefaultKeyLifetime(TimeSpan.FromDays(60));


        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
            CookieAuthenticationDefaults.AuthenticationScheme, opts =>
            {
                opts.LoginPath = "/Auth/SignIn";
                opts.ExpireTimeSpan = TimeSpan.FromDays(60);
                opts.SlidingExpiration = true;
                opts.Cookie.Name = "webCookie";
                opts.AccessDeniedPath = "/AccessDenied";
            });

        return services;
    }
}
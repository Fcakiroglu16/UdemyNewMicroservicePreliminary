using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceLocalizationExt
    {
        public static IServiceCollection AddLocalizationExt(this IServiceCollection services)
        {
            var supportedCultures = new CultureInfo[] { new("tr-Tr") };


            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SupportedCultures = supportedCultures; // datetime-currency
                options.SupportedUICultures = supportedCultures; // string localization
                options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
            });

            return services;
        }
    }
}
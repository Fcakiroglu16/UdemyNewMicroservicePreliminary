using Microsoft.Extensions.Options;
using UdemyMicroservices.Web.Options;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceOptionExt
    {
        public static IServiceCollection AddOptionsExt(this IServiceCollection services)
        {
            services
                .AddOptions<IdentityOption>()
                .BindConfiguration(nameof(IdentityOption))
                .ValidateDataAnnotations()
                .ValidateOnStart();


            services
                .AddOptions<FileServiceOption>()
                .BindConfiguration(nameof(FileServiceOption))
                .ValidateDataAnnotations()
                .ValidateOnStart();


            services.AddSingleton(sp => sp.GetRequiredService<IOptions<FileServiceOption>>().Value);

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<IdentityOption>>().Value);

            return services;
        }
    }
}
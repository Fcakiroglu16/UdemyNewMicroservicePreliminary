using UdemyMicroservices.Web.DelegatingHandlers;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceHttpHandlerExt
    {
        public static IServiceCollection AddHttpHandlerExt(this IServiceCollection services)
        {
            services.AddScoped<AuthenticatedHttpClientHandler>();
            services.AddScoped<ClientAuthenticatedHttpClientHandler>();

            return services;
        }
    }
}
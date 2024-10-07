namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceCacheExt
    {
        public static IServiceCollection AddCacheExt(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            return services;
        }
    }
}
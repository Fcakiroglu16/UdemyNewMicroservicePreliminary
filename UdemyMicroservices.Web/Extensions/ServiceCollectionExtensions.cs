using UdemyMicroservices.Web.Services;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAllServicesExt(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<ITransientService>()
                .AddClasses(classes => classes.AssignableTo<ITransientService>())
                .AsSelf()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo<IScopedService>())
                .AsSelf()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ISingletonService>())
                .AsSelf()
                .WithSingletonLifetime());
        }
    }
}
using Microsoft.Extensions.Options;
using UdemyMicroservices.Catalog.Repositories;

namespace UdemyMicroservices.Catalog
{
    public static class OptionsExt
    {
        public static IServiceCollection AddOptionsExt(this IServiceCollection services)
        {
            /*
AddOptions<MongoOption>(): MongoOption yapılandırma sınıfını dependency injection'a ekler.
BindConfiguration(nameof(MongoOption)): MongoOption sınıfını appsettings.json gibi yapılandırma kaynaklarına bağlar.
ValidateDataAnnotations(): MongoOption sınıfındaki DataAnnotation doğrulamalarını uygular.
ValidateOnStart(): Uygulama başlatılırken yapılandırma verilerini doğrular.
 */
            services
                .AddOptions<MongoOption>()
                .BindConfiguration(nameof(MongoOption))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoOption>>().Value);

            return services;
        }
    }
}
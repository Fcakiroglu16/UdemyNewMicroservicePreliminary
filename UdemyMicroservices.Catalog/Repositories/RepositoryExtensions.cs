using MongoDB.Driver;

namespace UdemyMicroservices.Catalog.Repositories;

public static class RepositoryExtensions
{
    public static IServiceCollection AddDatabaseServicesExt(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp =>
        {
            var mongoOption = sp.GetRequiredService<MongoOption>();
            return new MongoClient(mongoOption.ConnectionString);
        });

        services.AddScoped(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var mongoOption = sp.GetRequiredService<MongoOption>();
            return AppDbContext.Create(mongoClient.GetDatabase(mongoOption.DatabaseName));
        });

        return services;
    }
}
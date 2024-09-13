using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UdemyMicroservices.Order.Application.Contracts.Persistence;
using UdemyMicroservices.Order.Persistence.Repositories;
using UdemyMicroservices.Order.Repository;

namespace UdemyMicroservices.Order.Persistence;

public static class PersistenceExt
{
    public static IServiceCollection AddPersistenceExt(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
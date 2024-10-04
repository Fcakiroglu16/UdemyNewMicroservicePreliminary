using MassTransit;
using UdemyMicroservices.Shared.Options;

namespace UdemyMicroservices.Order.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMasstransitExt(this IServiceCollection services,
            IConfiguration configuration)
        {
            var busOptions = configuration.GetSection(nameof(BusOption)).Get<BusOption>();


            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(busOptions!.RabbitMq.Address, hostConfigure =>
                    {
                        hostConfigure.Username(busOptions.RabbitMq.UserName);
                        hostConfigure.Password(busOptions.RabbitMq.Password);
                    });
                });
            });


            return services;
        }
    }
}
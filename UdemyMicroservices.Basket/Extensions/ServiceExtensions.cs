using MassTransit;
using UdemyMicroservices.Basket.Consumers;
using UdemyMicroservices.Shared.Options;

namespace UdemyMicroservices.Basket.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddMasstransitExt(this IServiceCollection services,
        IConfiguration configuration)
    {
        var busOptions = configuration.GetSection(nameof(BusOption)).Get<BusOption>();


        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateOrderEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(busOptions!.RabbitMq.Address, hostConfigure =>
                {
                    hostConfigure.Username(busOptions.RabbitMq.UserName);
                    hostConfigure.Password(busOptions.RabbitMq.Password);
                });

                cfg.ReceiveEndpoint("basket-microservice.create-order-event.queue",
                    e => { e.ConfigureConsumer<CreateOrderEventConsumer>(context); });
            });
        });


        return services;
    }
}
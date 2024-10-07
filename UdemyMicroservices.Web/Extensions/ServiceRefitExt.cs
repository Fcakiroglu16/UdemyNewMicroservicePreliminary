using System.Reflection;
using Refit;
using UdemyMicroservices.Web.DelegatingHandlers;
using UdemyMicroservices.Web.Services.Refit;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceRefitExt
    {
        public static IServiceCollection AddRefitServiceExt(this IServiceCollection services,
            IConfiguration configuration)
        {
            #region without reflection

            //builder.Services.AddRefitClient<IRefitCatalogService>()
            //    .ConfigureHttpClient(
            //        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
            //    .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>()
            //    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
            //builder.Services.AddRefitClient<IRefitFileService>()
            //    .ConfigureHttpClient(
            //        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
            //    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();


            //builder.Services.AddRefitClient<IRefitBasketService>()
            //    .ConfigureHttpClient(
            //        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
            //    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();


            //builder.Services.AddRefitClient<IRefitDiscountService>()
            //    .ConfigureHttpClient(
            //        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
            //    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

            //builder.Services.AddRefitClient<IRefitOrderService>()
            //    .ConfigureHttpClient(
            //        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
            //    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

            #endregion


            var gatewayBaseAddress = new Uri(configuration.GetSection("GatewayServiceOption")["Address"]!);

            var refitInterfaces = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsInterface && t.Name.StartsWith("IRefit"));


            foreach (var serviceType in refitInterfaces)
            {
                var refitClientBuilder = services.AddRefitClient(serviceType)
                    .ConfigureHttpClient(c => c.BaseAddress = gatewayBaseAddress)
                    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

                if (serviceType == typeof(IRefitCatalogService))
                {
                    refitClientBuilder.AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();
                }
            }

            return services;
        }
    }
}
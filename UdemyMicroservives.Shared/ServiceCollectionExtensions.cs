using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace UdemyMicroservices.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServicesExt(this IServiceCollection services, Type assembly)
        {
            services.AddAutoMapper(assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(assembly);
            services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(assembly));

            return services;
        }
    }
}
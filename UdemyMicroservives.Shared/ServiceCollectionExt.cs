using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace UdemyMicroservices.Shared;

public static class ServiceCollectionExt
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
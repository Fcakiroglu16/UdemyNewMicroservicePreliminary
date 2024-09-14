using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Shared;

public static class ServiceCollectionExt
{
    public static IServiceCollection AddCommonServicesExt(this IServiceCollection services, Type assembly)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddAutoMapper(assembly);
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(assembly);
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(assembly));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}
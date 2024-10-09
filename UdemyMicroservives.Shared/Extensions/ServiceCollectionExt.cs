using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UdemyMicroservices.Shared.Options;
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

        services
            .AddOptions<IdentityServerOption>()
            .BindConfiguration(nameof(IdentityServerOption))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<BusOption>()
            .BindConfiguration(nameof(BusOption))
            .ValidateDataAnnotations()
            .ValidateOnStart();


        services
            .AddOptions<MicroserviceOption>()
            .BindConfiguration(nameof(MicroserviceOption))
            .ValidateDataAnnotations()
            .ValidateOnStart();


        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MicroserviceOption>>().Value);

        return services;
    }

    public static IServiceCollection AddAuthenticationExt(this IServiceCollection services,
        IConfiguration configuration)
    {
        var identityOption = configuration.GetSection(nameof(IdentityServerOption)).Get<IdentityServerOption>();


        services.AddAuthentication().AddJwtBearer(
            JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = identityOption!.Address;
                options.Audience = identityOption!.Audience;
                options.RequireHttpsMetadata = false;

                //options.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateIssuer = false
                //};
            });


        return services;
    }
}
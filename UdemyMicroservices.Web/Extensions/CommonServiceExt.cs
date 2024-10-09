using Microsoft.AspNetCore.Mvc;

namespace UdemyMicroservices.Web.Extensions;

public static class CommonServiceExt
{
    public static IServiceCollection AddCommonWebServicesExt(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
        services.AddRazorPages();
        services.AddMvc(opt => { opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; });
        services.AddHttpContextAccessor();

        return services;
    }
}
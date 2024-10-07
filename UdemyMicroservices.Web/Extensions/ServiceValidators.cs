using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceValidators
    {
        public static IServiceCollection AddValidatorExt(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
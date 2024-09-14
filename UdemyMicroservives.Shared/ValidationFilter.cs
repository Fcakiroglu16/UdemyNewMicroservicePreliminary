using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace UdemyMicroservices.Shared
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator == null) return await next(context);


            var entity = context.Arguments.OfType<T>().FirstOrDefault();
            if (entity == null)
            {
                return Results.Problem("Error Not Found");
            }

            var results = await validator.ValidateAsync(entity);
            if (!results.IsValid)
            {
                return Results.ValidationProblem(results.ToDictionary());
            }

            return await next(context);
        }
    }
}
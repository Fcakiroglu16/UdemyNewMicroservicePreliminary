using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UdemyMicroservices.Shared
{
    public static class SwaggerExt
    {
        public static IServiceCollection AddSwaggerServicesExt(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }


        public static void AddSwaggerMiddlewareExt(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                    {
                        var descriptions = app.DescribeApiVersions();
                        foreach (var description in descriptions)
                        {
                            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });
            }
        }
    }
}
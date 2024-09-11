using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UdemyMicroservices.Shared
{
    public static class VersioningExt
    {
        public static IServiceCollection AddVersioningExt(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1.0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                //options.ApiVersionReader = ApiVersionReader.Combine(
                //    new UrlSegmentApiVersionReader(),
                //    new HeaderApiVersionReader("X-Api-Version"),
                //    new QueryStringApiVersionReader());
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }


        public static ApiVersionSet AddVersionSetExt(this WebApplication app)
        {
            var apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1.0))
                .HasApiVersion(new ApiVersion(2.0))
                .ReportApiVersions()
                .Build();

            return apiVersionSet;
        }
    }
}
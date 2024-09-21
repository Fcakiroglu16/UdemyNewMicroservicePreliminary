using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using UdemyMicroservices.Web.Pages.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorOptions(options =>
{
    //options.ViewLocationFormats.Add("/Pages/Auth/{0}.cshtml");
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services
    .AddOptions<IdentityOption>()
    .BindConfiguration(nameof(IdentityOption))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<IdentityOption>>().Value);


builder.Services.AddHttpClient<AuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
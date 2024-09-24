using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Refit;
using System.Reflection;
using UdemyMicroservices.Web.Options;
using UdemyMicroservices.Web.Pages.Auth.Options;
using UdemyMicroservices.Web.Pages.Auth.SignIn;
using UdemyMicroservices.Web.Pages.Auth.SignUp;
using UdemyMicroservices.Web.Pages.Instructor.CreateCourse;
using UdemyMicroservices.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddMvc(opt => { opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; });


builder.Services.AddHttpContextAccessor();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services
    .AddOptions<IdentityOption>()
    .BindConfiguration(nameof(IdentityOption))
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services
    .AddOptions<FileServiceOption>()
    .BindConfiguration(nameof(FileServiceOption))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<FileServiceOption>>().Value);


builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<IdentityOption>>().Value);


builder.Services.AddHttpClient<SignUpService>();
builder.Services.AddHttpClient<SignInService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme, opts =>
    {
        opts.LoginPath = "/Auth/SignIn";
        opts.ExpireTimeSpan = TimeSpan.FromDays(60);
        opts.SlidingExpiration = true;
        opts.Cookie.Name = "webCookie";
    });


builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(
        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!));
builder.Services.AddRefitClient<IFileService>()
    .ConfigureHttpClient(
        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!));


builder.Services.AddScoped<CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using UdemyMicroservices.Web.Pages.Auth.Options;
using UdemyMicroservices.Web.Pages.Auth.SignIn;
using UdemyMicroservices.Web.Pages.Auth.SignUp;

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
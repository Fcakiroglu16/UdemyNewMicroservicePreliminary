using System.Globalization;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Refit;
using UdemyMicroservices.Web.DelegatingHandlers;
using UdemyMicroservices.Web.Options;
using UdemyMicroservices.Web.Pages.Auth.SignIn;
using UdemyMicroservices.Web.Pages.Auth.SignUp;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.Services.Refit;

var builder = WebApplication.CreateBuilder(args);


var supportedCultures = new CultureInfo[] { new("tr-Tr") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SupportedCultures = supportedCultures; // datetime-currency
    options.SupportedUICultures = supportedCultures; // string localization
    options.DefaultRequestCulture = new
        RequestCulture(supportedCultures.First());
});


builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

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
        opts.AccessDeniedPath = "/AccessDenied";
    });
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddScoped<ClientAuthenticatedHttpClientHandler>();


builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(
        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
    .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>()
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();
builder.Services.AddRefitClient<IFileService>()
    .ConfigureHttpClient(
        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();


builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(
        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();


builder.Services.AddRefitClient<IDiscountService>()
    .ConfigureHttpClient(
        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

builder.Services.AddRefitClient<IOrderService>()
    .ConfigureHttpClient(
        c => c.BaseAddress = new Uri(builder.Configuration.GetSection("GatewayServiceOption")["Address"]!))
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();


builder.Services.AddScoped<CatalogService>();
builder.Services.AddScoped<BasketService>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SampleInstance";
});

var app = builder.Build();
app.UseRequestLocalization();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
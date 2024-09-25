using Microsoft.AspNetCore.Authentication.JwtBearer;
using UdemyMicroservices.Basket.Features.Basket;
using UdemyMicroservices.Catalog;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthenticationExt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerServicesExt();
builder.Services.AddCommonServicesExt(typeof(BasketAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SampleInstance";
});

var app = builder.Build();
app.UseExceptionHandler();

app.AddSwaggerMiddlewareExt();
app.UseAuthentication();
app.UseAuthorization();
var apiVersionSet = app.AddVersionSetExt();
app.AddBasketEndpointsExt(apiVersionSet);


app.Run();
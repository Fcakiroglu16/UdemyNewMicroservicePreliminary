using UdemyMicroservices.Basket.Features.Basket;
using UdemyMicroservices.Catalog;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);


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
var apiVersionSet = app.AddVersionSetExt();
app.AddBasketEndpointsExt(apiVersionSet);


app.Run();
using UdemyMicroservices.Catalog;
using UdemyMicroservices.Catalog.Repositories;
using UdemyMicroservices.Discount.Features.Discount;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerServicesExt();
builder.Services.AddCommonServicesExt(typeof(DiscountAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddOptionsExt();
builder.Services.AddDatabaseServicesExt();

var app = builder.Build();
app.UseExceptionHandler();
app.AddSwaggerMiddlewareExt();
var apiVersionSet = app.AddVersionSetExt();
app.AddDiscountEndpointsExt(apiVersionSet);
app.Run();
using UdemyMicroservices.Catalog;
using UdemyMicroservices.Order.API.Endpoints.Order;
using UdemyMicroservices.Order.Persistence;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthenticationExt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerServicesExt();
builder.Services.AddCommonServicesExt(typeof(OrderApplicationAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddPersistenceExt(builder.Configuration);
var app = builder.Build();
app.UseExceptionHandler();
app.AddSwaggerMiddlewareExt();


app.UseAuthentication();
app.UseAuthorization();
var apiVersionSet = app.AddVersionSetExt();
app.AddOrderEndpointsExt(apiVersionSet);


app.Run();
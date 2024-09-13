using UdemyMicroservices.Catalog;
using UdemyMicroservices.Order.API.Endpoints.Order;
using UdemyMicroservices.Order.Persistence;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerServicesExt();
builder.Services.AddCommonServicesExt(typeof(OrderApplicationAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddPersistenceExt(builder.Configuration);
var app = builder.Build();

app.AddSwaggerMiddlewareExt();
var apiVersionSet = app.AddVersionSetExt();
app.AddOrderEndpointsExt(apiVersionSet);


app.Run();
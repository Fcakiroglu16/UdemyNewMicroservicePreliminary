using Refit;
using UdemyMicroservices.Catalog;
using UdemyMicroservices.Order.API.Endpoints.Order;
using UdemyMicroservices.Order.API.Extensions;
using UdemyMicroservices.Order.Application.Contracts.refit;
using UdemyMicroservices.Order.Application.DelegateHandlers;
using UdemyMicroservices.Order.Persistence;
using UdemyMicroservices.Shared;
using UdemyMicroservices.Shared.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthenticationExt(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerServicesExt();
builder.Services.AddCommonServicesExt(typeof(OrderApplicationAssembly));
builder.Services.AddVersioningExt();
builder.Services.AddPersistenceExt(builder.Configuration);
builder.Services.AddMasstransitExt(builder.Configuration);

builder.Services.AddScoped<AuthenticatedHttpClientHandler>();

builder.Services.AddRefitClient<IPaymentService>()
    .ConfigureHttpClient(
        c =>
        {
            var microserviceOption =
                builder.Configuration.GetSection(nameof(MicroserviceOption)).Get<MicroserviceOption>();
            c.BaseAddress = new Uri(microserviceOption!.Payment!.Address);
        }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

var app = builder.Build();
app.UseExceptionHandler();
app.AddSwaggerMiddlewareExt();


app.UseAuthentication();
app.UseAuthorization();
var apiVersionSet = app.AddVersionSetExt();
app.AddOrderEndpointsExt(apiVersionSet);


app.Run();
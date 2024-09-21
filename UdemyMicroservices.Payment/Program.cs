using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Payment;
using UdemyMicroservices.Payment.Features.Payments;
using UdemyMicroservices.Payment.Repositories;
using UdemyMicroservices.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerServicesExt();
builder.Services.AddCommonServicesExt(typeof(PaymentAssembly));
builder.Services.AddVersioningExt();

builder.Services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("payment-db"); });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
var app = builder.Build();
app.UseExceptionHandler();
app.AddSwaggerMiddlewareExt();
var apiVersionSet = app.AddVersionSetExt();
app.AddPaymentEndpointsExt(apiVersionSet);


app.Run();
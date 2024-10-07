using UdemyMicroservices.Web.ExceptionHandler;
using UdemyMicroservices.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonWebServicesExt();
builder.Services.AddLocalizationExt();
builder.Services.AddValidatorExt();
builder.Services.AddOptionsExt();
builder.Services.AddHttpClientsExt();
builder.Services.AddAuthenticationExt();
builder.Services.AddHttpHandlerExt();
builder.Services.AddAllServicesExt();
builder.Services.AddRefitServiceExt(builder.Configuration);
builder.Services.AddCacheExt(builder.Configuration);
builder.Services.AddExceptionHandler<UnAuthorizeExceptionHandler>();
var app = builder.Build();


app.AddMiddlewares();
app.MapRazorPages();
app.Run();
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using UdemyMicroservices.Web.ExceptionHandler;
using UdemyMicroservices.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCommonWebServicesExt();
builder.Services.AddCustomLocalizationExt();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddOptionsExt();
builder.Services.AddHttpClientsExt();
builder.Services.AddAuthenticationExt();
builder.Services.AddHttpHandlerExt();
builder.Services.AddRefitServiceExt(builder.Configuration);
builder.Services.AddAllServicesExt();
builder.Services.AddCacheExt(builder.Configuration);
builder.Services.AddExceptionHandler<UnAuthorizeExceptionHandler>();
var app = builder.Build();
app.UseRequestLocalization();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
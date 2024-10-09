namespace UdemyMicroservices.Web.Extensions;

public static class ServicesMiddlewareExt
{
    public static void AddMiddlewares(this WebApplication app)
    {
        app.UseRequestLocalization();


        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/Error");
        else
            app.UseExceptionHandler("/Error");


        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
    }
}
using Microsoft.AspNetCore.Diagnostics;

namespace UdemyMicroservices.Web.ExceptionHandler;

public class UnAuthorizeExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is UnauthorizedAccessException) httpContext.Response.Redirect("Auth/Signin");


        return ValueTask.FromResult(true);
    }
}
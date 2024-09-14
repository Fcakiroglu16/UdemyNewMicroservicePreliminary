using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UdemyMicroservices.Shared
{
    public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            //var errorAsDto = ServiceResult.Fail(exception.Message, HttpStatusCode.InternalServerError);

            //ProblemDetails x = new ProblemDetails();

            //x.httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //httpContext.Response.ContentType = "application/json";
            //await httpContext.Response.WriteAsJsonAsync(errorAsDto, cancellationToken: cancellationToken);
            //httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            //{
            //    Title = "An error occurred",
            //    Detail = exception.Message,
            //    Type = exception.GetType().Name,
            //    Status = (int)HttpStatusCode.InternalServerError
            //}, cancellationToken: cancellationToken);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Title = "An error occurred",
                    Detail = exception.Message,
                    Type = exception.GetType().Name,
                },
                Exception = exception
            });

            return true;
        }
    }
}
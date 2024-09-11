using System.Net;
using Microsoft.AspNetCore.Http;

namespace UdemyMicroservices.Shared;

public static class ResultExt
{
    public static IResult ToActionResult<T>(this ServiceResult<T> result)
    {
        return result.Status switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.Created => Results.Created(result.UrlAsCreated, result),
            HttpStatusCode.NoContent => Results.NoContent(),
            HttpStatusCode.NotFound => Results.NotFound(result.Fail),
            HttpStatusCode.BadRequest => Results.BadRequest(result.Fail),
            _ => throw new ArgumentOutOfRangeException(nameof(result.Status))
        };
    }

    public static IResult ToActionResult(this ServiceResult result)
    {
        return result.Status switch
        {
            HttpStatusCode.NoContent => Results.NoContent(),
            HttpStatusCode.NotFound => Results.NotFound(result.Fail),
            HttpStatusCode.BadRequest => Results.BadRequest(result.Fail),
            _ => throw new ArgumentOutOfRangeException(nameof(result.Status))
        };
    }
}
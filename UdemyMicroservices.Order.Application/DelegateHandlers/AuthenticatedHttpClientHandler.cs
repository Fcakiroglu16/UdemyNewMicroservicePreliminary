using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace UdemyMicroservices.Order.Application.DelegateHandlers;

public class AuthenticatedHttpClientHandler(IHttpContextAccessor? contextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (contextAccessor is null) return await base.SendAsync(request, cancellationToken);


        if (!contextAccessor.HttpContext!.User.Identity!.IsAuthenticated)
            return await base.SendAsync(request, cancellationToken);


        string? accessToken = null;


        if (contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var values))
            accessToken = values.ToString().Split(" ")[1];


        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


        return await base.SendAsync(request, cancellationToken);
    }
}
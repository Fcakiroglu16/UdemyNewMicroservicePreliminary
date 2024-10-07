using System.Net;
using System.Net.Http.Headers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UdemyMicroservices.Web.Services;

namespace UdemyMicroservices.Web.DelegatingHandlers;

public class AuthenticatedHttpClientHandler(IHttpContextAccessor? contextAccessor, TokenService userService)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!contextAccessor!.HttpContext!.User.Identity!.IsAuthenticated)
            return await base.SendAsync(request, cancellationToken);

        var accessToken = await contextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        if (accessToken is null)
        {
            contextAccessor.HttpContext.Response.Redirect("/Auth/SignIn");
        }

        if (!string.IsNullOrEmpty(accessToken))
            request.SetBearerToken(accessToken);


        var response = await base.SendAsync(request, cancellationToken);


        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;


        var refreshToken =
            await contextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);


        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedAccessException();
        }


        var tokenResponse = await userService.GetAccessTokenByRefreshToken();

        if (tokenResponse.IsFail)
        {
            throw new UnauthorizedAccessException();
        }


        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Data!.AccessToken);

        response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}
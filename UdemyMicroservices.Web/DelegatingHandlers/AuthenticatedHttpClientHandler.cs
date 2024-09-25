using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace UdemyMicroservices.Web.DelegatingHandlers
{
    public class AuthenticatedHttpClientHandler(IHttpContextAccessor? contextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (contextAccessor is null) return await base.SendAsync(request, cancellationToken);


            var accessToken =
                (await contextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.AccessToken))!;


            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }


            return await base.SendAsync(request, cancellationToken);
        }
    }
}
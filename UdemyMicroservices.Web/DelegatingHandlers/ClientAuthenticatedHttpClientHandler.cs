using System.Net.Http.Headers;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Web.Options;

namespace UdemyMicroservices.Web.DelegatingHandlers;

public class ClientAuthenticatedHttpClientHandler(
    IHttpContextAccessor? contextAccessor,
    HttpClient client,
    IdentityOption identityOption,
    ILogger<ClientAuthenticatedHttpClientHandler> logger,
    IDistributedCache distributedCache) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (contextAccessor!.HttpContext!.User.Identity!.IsAuthenticated)
            return await base.SendAsync(request, cancellationToken);


        var responseAsDiscovery =
            await client.GetDiscoveryDocumentAsync(identityOption.Tenant.Address, cancellationToken);


        if (responseAsDiscovery.IsError)
        {
            logger.LogError(responseAsDiscovery.Error, "Failed to retrieve discovery document.");
            throw new Exception("A system error occurred. Please try again later.");
        }

        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = responseAsDiscovery.TokenEndpoint,
            ClientId = identityOption.Tenant.ClientId,
            ClientSecret = identityOption.Tenant.ClientSecret
            //Scope = "webshoppingagg.fullpermission"
        };

        var tokenResponse =
            await client.RequestClientCredentialsTokenAsync(tokenRequest, cancellationToken);

        if (tokenResponse.IsError)
        {
            logger.LogError(tokenResponse.Error, "Failed to retrieve access token.");
            throw new Exception("A system error occurred. Please try again later.");
        }


        await distributedCache.SetStringAsync("AccessToken", tokenResponse.AccessToken!,
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) },
            cancellationToken);


        if (contextAccessor is null) return await base.SendAsync(request, cancellationToken);


        if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}
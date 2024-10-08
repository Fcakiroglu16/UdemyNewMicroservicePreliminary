using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using UdemyMicroservices.Web.Options;
using UdemyMicroservices.Web.Pages.Auth.SignIn;
using UdemyMicroservices.Web.ViewModels;


namespace UdemyMicroservices.Web.Services;

public class TokenService(
    IHttpContextAccessor contextAccessor,
    HttpClient client,
    IdentityOption identityOption,
    ILogger<TokenService> logger,
    IDistributedCache distributedCache) : IHttpClientService
{
    private const string DiscoveryResponseCacheKey = "DiscoveryResponseKey";


    public async Task<ServiceResult<TokenResponse>> GetAccessTokenByRefreshToken()
    {
        var refreshToken = await contextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        if (string.IsNullOrEmpty(refreshToken))
            return ServiceResult<TokenResponse>.Fail("No RefreshToken provided");


        var responseAsDiscovery = await client.GetDiscoveryDocumentAsync(identityOption.Tenant.Address);

        if (responseAsDiscovery.IsError)
        {
            logger.LogError(responseAsDiscovery.Error, "Failed to retrieve discovery document.");
            return ServiceResult<TokenResponse>.Fail("A system error occurred. Please try again later.");
        }


        var tokenRequest = new RefreshTokenRequest
        {
            Address = responseAsDiscovery.TokenEndpoint,
            ClientId = identityOption.Tenant.ClientId,
            ClientSecret = identityOption.Tenant.ClientSecret,
            RefreshToken = refreshToken
        };


        var tokenResponse = await client.RequestRefreshTokenAsync(tokenRequest);

        if (tokenResponse.IsError)
        {
            logger.LogError(tokenResponse.Error, "Failed to retrieve access token by refresh token.");
            return ServiceResult<TokenResponse>.Fail("A system error occurred. Please try again later.");
        }


        var accessToken = tokenResponse.AccessToken;


        var identity = new ClaimsIdentity(contextAccessor.HttpContext!.User.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            "preferred_username", ClaimTypes.Role);

        var principal = new ClaimsPrincipal(identity);


        var authenticationProperties = CreateAuthenticationProperties(tokenResponse);


        await contextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            authenticationProperties);

        return ServiceResult<TokenResponse>.Success(tokenResponse);
    }


    public async Task<DiscoveryDocumentResponse> GetDiscovery(CancellationToken cancellationToken = default)
    {
        var discoveryRequest = new DiscoveryDocumentRequest
        {
            Address = identityOption.Tenant.Address,
            Policy = { RequireHttps = false }
        };


        var responseAsDiscovery =
            await client.GetDiscoveryDocumentAsync(discoveryRequest, cancellationToken);


        if (!responseAsDiscovery.IsError)
        {
            return responseAsDiscovery;
        }


        logger.LogError(responseAsDiscovery.Error, "Failed to retrieve discovery document.");
        throw new Exception("A system error occurred. Please try again later.");
    }


    public AuthenticationProperties CreateAuthenticationProperties(TokenResponse tokenResponse)
    {
        var authenticationTokens = new List<AuthenticationToken>
        {
            new()
            {
                Name = OpenIdConnectParameterNames.AccessToken,
                Value = tokenResponse.AccessToken!
            },
            new()
            {
                Name = OpenIdConnectParameterNames.RefreshToken,
                Value = tokenResponse.RefreshToken!
            },
            new()
            {
                Name = OpenIdConnectParameterNames.IdToken,
                Value = tokenResponse.IdentityToken!
            },
            new()
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
                    .ToString("o", CultureInfo.InvariantCulture)
            }
        };

        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.StoreTokens(authenticationTokens);

        return authenticationProperties;
    }


    public List<Claim> ExtractClaims(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);


        var claims = jwtSecurityToken.Claims.Select(claim => new Claim(claim.Type, claim.Value)).ToList();


        var hasRealmAccess = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "realm_access");


        if (hasRealmAccess is null) return claims;


        var roleAsClaim = JsonSerializer.Deserialize<RoleAsClaim>(hasRealmAccess.Value);


        if (roleAsClaim is null) return claims;


        claims.AddRange(roleAsClaim!.Roles!.Select(role => new Claim(ClaimTypes.Role, role)));


        return claims;
    }


    public async Task<List<Claim>> SetUserInfoClaims(DiscoveryDocumentResponse responseAsDiscovery,
        string accessToken, List<Claim> claims)
    {
        var response = await client.GetUserInfoAsync(new UserInfoRequest
        {
            Address = responseAsDiscovery.UserInfoEndpoint,
            Token = accessToken
        });


        foreach (var claim in response.Claims)
        {
            if (claims.Any(x => x.Type == claim.Type)) continue;

            claims.Add(new Claim(claim.Type, claim.Value));
        }

        return claims;
    }
}
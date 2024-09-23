using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using UdemyMicroservices.Web.Pages.Auth.Options;
using UdemyMicroservices.Web.Shared;

namespace UdemyMicroservices.Web.Pages.Auth.SignIn;

public class SignInService(
    IHttpContextAccessor contextAccessor,
    HttpClient client,
    IdentityOption identityOption,
    ILogger<SignInService> logger)
{
    public async Task<ServiceResult> SignInAsync(SignInViewModel model)
    {
        var responseAsDiscovery = await client.GetDiscoveryDocumentAsync(identityOption.Tenant.Address);

        if (responseAsDiscovery.IsError)
        {
            logger.LogError(responseAsDiscovery.Error, "Failed to retrieve discovery document.");
            return ServiceResult.Fail("A system error occurred. Please try again later.");
        }

        var tokenResponse = await GetTokenAsync(responseAsDiscovery, model);

        if (tokenResponse.IsFail) return ServiceResult.Fail(tokenResponse.Error!);

        var accessToken = tokenResponse.Data!.AccessToken!;

        var authenticationTokens = CreateAuthenticationTokens(tokenResponse.Data!);


        var authenticationProperties = CreateAuthenticationProperties(authenticationTokens);


        var claims = ExtractClaims(accessToken);
        var userInfoResponse = await GetUserInfoResponse(responseAsDiscovery, accessToken);


        AddUserInfoClaims(userInfoResponse, claims);


        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
            "preferred_username", ClaimTypes.Role);


        var principal = new ClaimsPrincipal(identity);


        await contextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            authenticationProperties);

        return ServiceResult.Success();
    }

    private async Task<ServiceResult<TokenResponse>> GetTokenAsync(DiscoveryDocumentResponse responseAsDiscovery,
        SignInViewModel model)
    {
        var tokenRequest = new PasswordTokenRequest
        {
            Address = responseAsDiscovery.TokenEndpoint,
            ClientId = identityOption.Tenant.ClientId,
            ClientSecret = identityOption.Tenant.ClientSecret,
            UserName = model.Email,
            Password = model.Password
        };

        var tokenResponse = await client.RequestPasswordTokenAsync(tokenRequest);


        if (tokenResponse.IsError)
        {
            logger.LogError(tokenResponse.Error, "Failed to retrieve access token.");
            return ServiceResult<TokenResponse>.Fail(tokenResponse.ErrorDescription!);
        }

        return ServiceResult<TokenResponse>.Success(tokenResponse);
    }

    private List<AuthenticationToken> CreateAuthenticationTokens(TokenResponse tokenResponse)
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

        return authenticationTokens;
    }

    private AuthenticationProperties CreateAuthenticationProperties(List<AuthenticationToken> authenticationTokens)
    {
        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.StoreTokens(authenticationTokens);

        return authenticationProperties;
    }


    private List<Claim> ExtractClaims(string accessToken)
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

    private async Task<UserInfoResponse> GetUserInfoResponse(DiscoveryDocumentResponse responseAsDiscovery,
        string accessToken)
    {
        return await client.GetUserInfoAsync(new UserInfoRequest
        {
            Address = responseAsDiscovery.UserInfoEndpoint,
            Token = accessToken
        });
    }

    private void AddUserInfoClaims(UserInfoResponse userInfoResponse, List<Claim> claims)
    {
        foreach (var claim in userInfoResponse.Claims)
        {
            if (claims.Any(x => x.Type == claim.Type)) continue;

            claims.Add(new Claim(claim.Type, claim.Value));
        }
    }
}
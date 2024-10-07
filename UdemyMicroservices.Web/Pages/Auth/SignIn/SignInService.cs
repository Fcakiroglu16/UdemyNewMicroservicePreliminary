using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using UdemyMicroservices.Web.Options;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Auth.SignIn;

public class SignInService(
    TokenService tokenService,
    IHttpContextAccessor contextAccessor,
    HttpClient client,
    IdentityOption identityOption,
    ILogger<SignInService> logger) : IHttpClientService
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

        if (tokenResponse.IsError)
        {
            logger.LogError(tokenResponse.Error, "Failed to retrieve access token.");
            return ServiceResult<TokenResponse>.Fail(tokenResponse.ErrorDescription!);
        }

        var accessToken = tokenResponse.AccessToken!;

        var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse);


        var claims = tokenService.ExtractClaims(accessToken);

        await tokenService.SetUserInfoClaims(responseAsDiscovery, accessToken, claims);


        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,
            "preferred_username", ClaimTypes.Role);


        var principal = new ClaimsPrincipal(identity);


        await contextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            authenticationProperties);

        return ServiceResult.Success();
    }

    private async Task<TokenResponse> GetTokenAsync(DiscoveryDocumentResponse responseAsDiscovery,
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

        return await client.RequestPasswordTokenAsync(tokenRequest);
    }
}
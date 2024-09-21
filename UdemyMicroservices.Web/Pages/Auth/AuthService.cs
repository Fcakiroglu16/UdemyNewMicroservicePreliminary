using IdentityModel.Client;
using UdemyMicroservices.Web.Pages.Auth.SignUp;
using UdemyMicroservices.Web.Shared;

namespace UdemyMicroservices.Web.Pages.Auth;

public class AuthService(HttpClient client, IdentityOption identityOption, ILogger<AuthService> logger)
{
    public async Task<ServiceResult> SignUpAsync(SignUpViewModel model)
    {
        var tokenResponseAsMaster = await TokenResponseAsMaster();


        if (tokenResponseAsMaster.IsFail) return ServiceResult.Fail(tokenResponseAsMaster.Error!);


        client.SetBearerToken(tokenResponseAsMaster.Data!.AccessToken!);


        var userCreatedRequestModel = CreateUserRequestModel(model);


        var responseAsUserCreate =
            await client.PostAsJsonAsync($"{identityOption.Tenant.UserCreateEndpoint}", userCreatedRequestModel);


        if (!responseAsUserCreate.IsSuccessStatusCode)
        {
            var error = await responseAsUserCreate.Content.ReadAsStringAsync();
            logger.LogError("Failed to create user. Error: {error}", error);

            return ServiceResult.Fail("A system error occurred. Please try again later.");
        }

        return ServiceResult.Success();
    }

    private static UserCreateRequest CreateUserRequestModel(SignUpViewModel model)
    {
        var credentials = new List<Credential>
        {
            new("password", model.Password, false)
        };

        var userCreateRequest = new UserCreateRequest(model.UserName, true, model.FirstName, model.LastName,
            model.Email, true, credentials
        );

        return userCreateRequest;
    }


    private async Task<ServiceResult<TokenResponse>> TokenResponseAsMaster()
    {
        var discoAsMaster = await client.GetDiscoveryDocumentAsync(identityOption.MasterTenant.Address);
        if (discoAsMaster.IsError)
        {
            logger.LogError(discoAsMaster.Error, "Failed to retrieve discovery document.");

            return ServiceResult<TokenResponse>.Fail("A system error occurred. Please try again later.");
        }


        var tokenResponseAsMaster = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = discoAsMaster.TokenEndpoint,
            UserName = "admin",
            Password = "password",
            ClientId = identityOption.MasterTenant.ClientId
        });


        if (tokenResponseAsMaster.IsError)
        {
            logger.LogError(tokenResponseAsMaster.Error, "Failed to retrieve discovery document.");

            return ServiceResult<TokenResponse>.Fail("A system error occurred. Please try again later.");
        }

        return ServiceResult<TokenResponse>.Success(tokenResponseAsMaster);
    }
}
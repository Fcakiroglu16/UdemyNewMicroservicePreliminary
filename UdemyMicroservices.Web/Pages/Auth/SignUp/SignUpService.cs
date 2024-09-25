using IdentityModel.Client;
using UdemyMicroservices.Web.Options;
using UdemyMicroservices.Web.ViewModels;

namespace UdemyMicroservices.Web.Pages.Auth.SignUp;

public class SignUpService(HttpClient client, IdentityOption identityOption, ILogger<SignUpService> logger)
{
    public async Task<ServiceResult> SignUpAsync(SignUpViewModel model)
    {
        var tokenResponseAsMaster = await TokenResponseAsMaster();


        if (tokenResponseAsMaster.IsFail) return ServiceResult.Fail(tokenResponseAsMaster.ProblemDetails!);


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
            logger.LogError(discoAsMaster.Exception, "Failed to retrieve discovery document.");

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
            logger.LogError(tokenResponseAsMaster.Exception, "Failed to retrieve discovery document.");

            return ServiceResult<TokenResponse>.Fail("A system error occurred. Please try again later.");
        }

        return ServiceResult<TokenResponse>.Success(tokenResponseAsMaster);
    }
}
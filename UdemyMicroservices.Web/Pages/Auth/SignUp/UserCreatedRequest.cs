namespace UdemyMicroservices.Web.Pages.Auth.SignUp;

public record Credential(
    string Type,
    string Value,
    bool Temporary);

public record UserCreateRequest(
    string Username,
    bool Enabled,
    string FirstName,
    string LastName,
    string Email,
    bool EmailVerified,
    List<Credential> Credentials);
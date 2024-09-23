using System.Text.Json.Serialization;

namespace UdemyMicroservices.Web.Pages.Auth.SignIn;

public class RoleAsClaim
{
    [JsonPropertyName("roles")] public List<string>? Roles { get; set; }
}
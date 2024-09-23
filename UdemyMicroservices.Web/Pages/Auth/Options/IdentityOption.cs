using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Web.Pages.Auth.Options;

public class IdentityOption
{
    [Required] public IdentityOptionItemAsTenant Tenant { get; set; } = default!;
    [Required] public IdentityOptionItemAsMaster MasterTenant { get; set; } = default!;
}

public class IdentityOptionItemAsMaster
{
    [Required] public string UserName { get; set; } = default!;

    [Required] public string Password { get; set; } = default!;

    [Required] public string Address { get; set; } = default!;

    [Required] public string ClientId { get; set; } = default!;

    [Required] public string ClientSecret { get; set; } = default!;
}

public class IdentityOptionItemAsTenant
{
    [Required] public string UserCreateEndpoint { get; set; } = default!;

    [Required] public string Address { get; set; } = default!;

    [Required] public string ClientId { get; set; } = default!;

    [Required] public string ClientSecret { get; set; } = default!;
}
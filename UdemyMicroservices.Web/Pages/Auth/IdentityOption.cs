using System.ComponentModel.DataAnnotations;

namespace UdemyMicroservices.Web.Pages.Auth
{
    public class IdentityOption
    {
        public IdentityOptionItemAsTenant Tenant { get; set; } = default!;
        public IdentityOptionItemAsMaster MasterTenant { get; set; } = default!;
    }

    public class IdentityOptionItemAsMaster
    {
        public string? UserName { get; set; } = default!;

        public string? Password { get; set; } = default!;

        [Required] public string Address { get; set; } = default!;

        [Required] public string ClientId { get; set; } = default!;

        [Required] public string ClientSecret { get; set; } = default!;
    }


    public class IdentityOptionItemAsTenant
    {
        public string UserCreateEndpoint { get; set; } = default!;

        [Required] public string Address { get; set; } = default!;

        [Required] public string ClientId { get; set; } = default!;

        [Required] public string ClientSecret { get; set; } = default!;
    }
}
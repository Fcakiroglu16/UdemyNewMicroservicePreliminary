using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace UdemyMicroservices.Web.Pages.Developer
{
    [Authorize]
    public class TokensModel : PageModel
    {
        public string AccessToken { get; set; } = default!;

        public string IdToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;

        public async Task OnGet()
        {
            AccessToken = (await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken))!;
            IdToken = (await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken))!;
            RefreshToken = (await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken))!;
        }
    }
}
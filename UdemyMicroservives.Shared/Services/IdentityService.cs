using Microsoft.AspNetCore.Http;

namespace UdemyMicroservices.Shared.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    //public string GetUserId => httpContextAccessor.HttpContext!.User.FindFirst("sub")!.Value;

    public string GetUserId => "1";
}
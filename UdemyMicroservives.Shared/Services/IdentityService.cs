using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UdemyMicroservices.Shared.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public Guid GetUserId =>
        Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public string GetFullName => httpContextAccessor.HttpContext!.User.FindFirst("name")!.Value;
}
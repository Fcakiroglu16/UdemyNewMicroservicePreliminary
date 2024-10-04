namespace UdemyMicroservices.Web.Services;

public class UserService(IHttpContextAccessor httpContextAccessor)
{
    public Guid GetUserId =>
        Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirst(x => x.Type == "sub")!.Value);
}
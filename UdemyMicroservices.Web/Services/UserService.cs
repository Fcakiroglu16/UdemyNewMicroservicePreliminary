namespace UdemyMicroservices.Web.Services
{
    public class UserService(IHttpContextAccessor contextAccessor)
    {
        public Guid GetUserId =>
            Guid.Parse(contextAccessor.HttpContext!.User.FindFirst(x => x.Type == "sub")!.Value);
    }
}
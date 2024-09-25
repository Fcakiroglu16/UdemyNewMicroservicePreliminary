namespace UdemyMicroservices.Shared.Services;

public interface IIdentityService
{
    public string GetUserId { get; }

    public string GetFullName { get; }
}
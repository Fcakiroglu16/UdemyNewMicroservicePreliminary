namespace UdemyMicroservices.Order.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
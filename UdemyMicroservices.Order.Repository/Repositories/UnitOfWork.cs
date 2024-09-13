using UdemyMicroservices.Order.Application.Contracts.Persistence;
using UdemyMicroservices.Order.Repository;

namespace UdemyMicroservices.Order.Persistence.Repositories;

internal class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}
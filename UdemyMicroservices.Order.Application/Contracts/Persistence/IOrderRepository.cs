namespace UdemyMicroservices.Order.Application.Contracts.Persistence;

public interface IOrderRepository : IGenericRepository<Domain.Entities.Order, Guid>
{
    Task<List<Domain.Entities.Order>> GetOrdersByUserIdAsync(Guid userId);
}
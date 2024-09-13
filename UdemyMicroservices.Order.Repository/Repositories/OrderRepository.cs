using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Order.Application.Contracts.Persistence;
using UdemyMicroservices.Order.Repository;

namespace UdemyMicroservices.Order.Persistence.Repositories;

internal class OrderRepository(AppDbContext context)
    : GenericRepository<Domain.Entities.Order, Guid>(context), IOrderRepository
{
    public Task<List<Domain.Entities.Order>> GetOrdersByUserIdAsync(string userId)
    {
        return Context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == userId).ToListAsync();
    }
}
using System.Linq.Expressions;

namespace UdemyMicroservices.Order.Application.Contracts.Persistence;

public interface IGenericRepository<T, in TId> where T : class where TId : struct
{
    public Task<bool> AnyAsync(TId id);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize);

    ValueTask<T?> GetByIdAsync(int id);
    ValueTask AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
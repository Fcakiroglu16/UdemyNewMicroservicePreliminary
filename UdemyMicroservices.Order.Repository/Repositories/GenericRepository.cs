using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UdemyMicroservices.Order.Application.Contracts.Persistence;
using UdemyMicroservices.Order.Domain.Entities;
using UdemyMicroservices.Order.Repository;

namespace UdemyMicroservices.Order.Persistence.Repositories;

public class GenericRepository<T, TId>(AppDbContext context)
    : IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    protected AppDbContext Context = context;


    public Task<bool> AnyAsync(TId id)
    {
        return _dbSet.AnyAsync(x => x.Id.Equals(id));
    }

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.AnyAsync(predicate);
    }

    public Task<List<T>> GetAllAsync()
    {
        return _dbSet.ToListAsync();
    }

    public Task<List<T>> GetAllPagedAsync(int pageNumber, int pageSize)
    {
        return _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }


    public ValueTask<T?> GetByIdAsync(int id)
    {
        return _dbSet.FindAsync(id);
    }

    public async ValueTask AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }


    public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).AsNoTracking();
    }
}
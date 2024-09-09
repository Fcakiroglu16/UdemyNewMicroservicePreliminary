using System.Linq.Expressions;

namespace UdemyMicroservices.Catalog.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string id);

        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(string id);
    }
}
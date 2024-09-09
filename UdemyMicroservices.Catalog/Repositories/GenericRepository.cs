using System.Linq.Expressions;
using MongoDB.Driver;

namespace UdemyMicroservices.Catalog.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IMongoCollection<T> Collection;

        public GenericRepository(MongoOption mongoOption)
        {
            var client = new MongoClient(mongoOption.ConnectionString);
            var db = client.GetDatabase(mongoOption.DatabaseName);
            Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public Task Delete(string id)
        {
            return Collection.FindOneAndDeleteAsync(x => x.Id == id);
        }

        public Task<T> GetById(string id)
        {
            return Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return Collection.Find(expression).AnyAsync();
        }

        public Task<List<T>> GetAll()
        {
            return Collection.AsQueryable().ToListAsync();
        }

        public Task Insert(T entity)
        {
            return Collection.InsertOneAsync(entity);
        }

        public Task Update(T entity)
        {
            return Collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
        }
    }
}
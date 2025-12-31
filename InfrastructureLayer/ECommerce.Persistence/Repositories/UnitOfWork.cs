using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.DbContexts;

namespace ECommerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _reposiotries = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var entityType = typeof(TEntity);

            if (_reposiotries.TryGetValue(entityType, out object? reposiotry))
                return (IGenericRepository<TEntity, TKey>)reposiotry;

            var newRepo = new GenericRepository<TEntity, TKey>(_dbContext);
            _reposiotries[entityType] = newRepo;

            return newRepo;
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}

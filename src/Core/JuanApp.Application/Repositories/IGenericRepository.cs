using System.Linq.Expressions;

namespace JuanApp.Application.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        // Query Methods
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking = true);
        Task<T?> GetByIdAsync(object id, bool tracking = false);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        // Write Methods
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<int> SaveAsync();
    }
}
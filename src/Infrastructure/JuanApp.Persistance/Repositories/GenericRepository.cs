using JuanApp.Application.Repositories;
using JuanApp.Domain.Common;
using JuanApp.Persistance.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JuanApp.Persistance.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly JuanAppContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(JuanAppContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            
            if (typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
                query = query.Cast<IBaseEntity>().Where(x => !x.IsDeleted).Cast<T>();
            
            return query;
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            var query = _dbSet.Where(predicate);
            if (!tracking)
                query = query.AsNoTracking();
            
            if (typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
                query = query.Cast<IBaseEntity>().Where(x => !x.IsDeleted).Cast<T>();
            
            return query;
        }

        public async Task<T?> GetByIdAsync(object id, bool tracking = true)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return null;

            if (!tracking)
                _context.Entry(entity).State = EntityState.Detached;

            if (entity is IBaseEntity baseEntity && baseEntity.IsDeleted)
                return null;

            return entity;
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            var query = _dbSet.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            if (typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
                query = query.Cast<IBaseEntity>().Where(x => !x.IsDeleted).Cast<T>();

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet.AsQueryable();
            
            if (typeof(IBaseEntity).IsAssignableFrom(typeof(T)))
                query = query.Cast<IBaseEntity>().Where(x => !x.IsDeleted).Cast<T>();

            return await query.AnyAsync(predicate);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Remove(T entity)
        {
            if (entity is IBaseEntity baseEntity)
            {
                baseEntity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
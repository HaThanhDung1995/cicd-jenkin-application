using DemoCICD.Domain.Abstractions.Entities;
using DemoCICD.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Persitence.Repositories
{
    public class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>, IDisposable
        where TEntity : DomainEntity<TKey>
    {

        private readonly ApplicationDbContext _context;
        private ILogger<RepositoryBase<TEntity, TKey>> _logger;
        public RepositoryBase(ApplicationDbContext context, ILogger<RepositoryBase<TEntity, TKey>> logger)
        {
            _context = context;
            _logger =logger;
        }
            

        public void Dispose()
        {
            _logger.LogWarning("Dispose");
            _context.Dispose();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
            if (includeProperties != null)
                foreach (var includeProperty in includeProperties)
                    items = items.Include(includeProperty);

            if (predicate is not null)
                items = items.Where(predicate);

            return items;
        }

        public async Task<TEntity> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
            => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

        public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
            => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(predicate, cancellationToken);

        public void Add(TEntity entity)
            => _context.Add(entity);

        public void Remove(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);

        public void RemoveMultiple(List<TEntity> entities)
            => _context.Set<TEntity>().RemoveRange(entities);

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);
    }
}

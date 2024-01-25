using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Domain.Abstractions
{
    public interface IUnitOfWork<TContext> : IAsyncDisposable where TContext : DbContext
    {
        /// <summary>
        /// Call save change from db context
        /// </summary>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        public TContext GetDbContext();
        
        
    }
}

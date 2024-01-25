using Castle.Core.Logging;
using DemoCICD.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Persitence
{
    public class EFUnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private ILogger<EFUnitOfWork<TContext>> _logger;

        public EFUnitOfWork(TContext context, ILogger<EFUnitOfWork<TContext>> logger)
        {
            _context = context;

            _logger = logger;
        }



        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync();

        async ValueTask IAsyncDisposable.DisposeAsync()
             {
            _logger.LogWarning("DisposeAsync");
            await _context.DisposeAsync();
                
                }

        TContext IUnitOfWork<TContext>.GetDbContext()
        {
            return _context;
        }
    }
}

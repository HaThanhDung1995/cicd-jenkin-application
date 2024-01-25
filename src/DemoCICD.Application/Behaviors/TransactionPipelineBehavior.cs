using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Repositories;
using DemoCICD.Persitence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace DemoCICD.Application.Behaviors
{
    public sealed class TransactionPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork; // SQL-SERVER-STRATEGY-2
        //private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1
        public TransactionPipelineBehavior(IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!IsCommand()) // In case TRequest is QueryRequest just ignore
                return await next();

            #region ============== SQL-SERVER-STRATEGY-1 ==============

            //// Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //// https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
            ///
            
            var strategy = _unitOfWork.GetDbContext().Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _unitOfWork.GetDbContext().Database.BeginTransactionAsync();
                {
                    var response = await next();
                    await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return response;
                }
            });
            #endregion ============== SQL-SERVER-STRATEGY-1 ==============

            #region ============== SQL-SERVER-STRATEGY-2 ==============

            //IMPORTANT: passing "TransactionScopeAsyncFlowOption.Enabled" to the TransactionScope constructor. This is necessary to be able to use it with async/await.

            //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            //    var response = await next();
            //    await _unitOfWork.SaveChangesAsync(cancellationToken);
            //    transaction.Complete();
            //    await _unitOfWork.DisposeAsync();
            //    return response;
            //}

            #endregion ============== SQL-SERVER-STRATEGY-2 ==============

        }

        private bool IsCommand()
            => typeof(TRequest).Name.EndsWith("Command");
    }
}

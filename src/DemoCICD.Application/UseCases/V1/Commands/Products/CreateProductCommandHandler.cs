using DemoCICD.Contract.Abstractions.Messages;
using DemoCICD.Contract.Services.Products;
using DemoCICD.Contract.Shared;
using DemoCICD.Domain.Abstractions.Repositories;
using DemoCICD.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DemoCICD.Application.Exceptions;
using DemoCICD.Domain.Exceptions;
using DemoCICD.Persitence;

namespace DemoCICD.Application.UseCases.V1.Commands.Products
{
    public class CreateProductCommandHandler : ICommandHandler<Command.CreateProductCommand>
    {
        private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;
        //private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork; // SQL-SERVER-STRATEGY-2
        //private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1
       

        public CreateProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
            //IUnitOfWork<ApplicationDbContext> unitOfWork,
            IPublisher publisher
            )
        {
            _productRepository = productRepository;
            //_unitOfWork = unitOfWork;
          
        }


        public async Task<Result> Handle(Command.CreateProductCommand request, CancellationToken cancellationToken)
        {
           
            #region First Product
            var product = Domain.Entities.Product.CreateProduct(Guid.NewGuid(), request.Name, request.Price, request.Description);
            _productRepository.Add(product);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);
            //await _context.SaveChangesAsync();
            #endregion
            #region Second Product from First Product
            //var productCreated = await _productRepository.FindByIdAsync(product.Id);

            //var productSecond = Domain.Entities.Product.CreateProduct(Guid.NewGuid(), productCreated.Name + " Second",
            //    productCreated.Price,
            //    productCreated.Id.ToString());

            //_productRepository.Add(productSecond);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);
            #endregion
            return Result.Success();
        }
    }
}

using DemoCICD.Contract.Abstractions.Messages;
using DemoCICD.Contract.Services.Products;
using DemoCICD.Contract.Shared;
using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Repositories;
using DemoCICD.Domain.Exceptions;
using DemoCICD.Persitence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.UseCases.V1.Commands.Products
{
    public sealed class DeleteProductCommandHandler : ICommandHandler<Command.DeleteProductCommand>
    {
        private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;
        private readonly IPublisher _publisher;
        public DeleteProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository, IPublisher publisher)
        //IUnitOfWork unitOfWork,
        //IPublisher publisher,
        //ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _publisher = publisher;
            //_unitOfWork = unitOfWork;
            //_context = context;
            //_publisher = publisher;
        }
        public async Task<Result> Handle(Command.DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(request.Id) ?? throw new ProductException.ProductNotFoundException(request.Id);
            _productRepository.Remove(product);
            await _publisher.Publish(new DomainEvent.ProductCreated(request.Id),cancellationToken);
            return Result.Success();
        }
    }
}

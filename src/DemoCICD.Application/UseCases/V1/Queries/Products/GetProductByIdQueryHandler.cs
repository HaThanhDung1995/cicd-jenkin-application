using AutoMapper;
using DemoCICD.Contract.Abstractions.Messages;
using DemoCICD.Contract.Services.Products;
using DemoCICD.Contract.Shared;
using DemoCICD.Domain.Abstractions.Repositories;
using DemoCICD.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.UseCases.V1.Queries.Products
{
    public sealed class GetProductByIdQueryHandler : IQueryHandler<Query.GetProductByIdQuery, Response.ProductResponse>
    {
        private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Result<Response.ProductResponse>> Handle(Query.GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(request.Id)
                ?? throw new ProductException.ProductNotFoundException(request.Id);

            var result = _mapper.Map<Response.ProductResponse>(product);

            return Result.Success(result);
        }
    }
}

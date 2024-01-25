using AutoMapper;
using DemoCICD.Contract.Abstractions.Messages;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
using DemoCICD.Contract.Services.Products;
using DemoCICD.Contract.Shared;
using DemoCICD.Domain.Abstractions.Repositories;
using DemoCICD.Domain.Entities;
using DemoCICD.Persitence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DemoCICD.Application.UseCases.V1.Queries.Products
{
    public class GetProductsQueryHandler : IQueryHandler<Query.GetProductQuery, PagedResult<Response.ProductResponse>>
    {
        private readonly ILogger<GetProductsQueryHandler> _logger;
        private readonly IRepositoryBase<Product,Guid> _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetProductsQueryHandler(IRepositoryBase<Product, Guid> productRepository, IMapper mapper, ILogger<GetProductsQueryHandler> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        
        private static Expression<Func<Domain.Entities.Product, object>> GetSortProperty(Query.GetProductQuery request)
         => request.SortColumn?.ToLower() switch
         {
             "name" => product => product.Name,
             "price" => product => product.Price,
             "description" => product => product.Description,
             _ => product => product.Id
             //_ => product => product.CreatedDate // Default Sort Descending on CreatedDate column
         };

        public async Task<Result<PagedResult<Response.ProductResponse>>> Handle(Query.GetProductQuery request, CancellationToken cancellationToken)
        {
            if (request.SortColumnAndOrder.Any())
            {
                var PageIndex = request.PageIndex <= 0 ? PagedResult<Domain.Entities.Product>.DefaultPageIndex : request.PageIndex;
                var PageSize = request.PageSize <= 0
                    ? PagedResult<Domain.Entities.Product>.DefaultPageSize
                    : request.PageSize > PagedResult<Domain.Entities.Product>.UpperPageSize
                    ? PagedResult<Domain.Entities.Product>.UpperPageSize : request.PageSize;

                // ============================================
                var productsQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
                    ? @$"SELECT * FROM {nameof(Domain.Entities.Product)} ORDER BY "
                    : @$"SELECT * FROM {nameof(Domain.Entities.Product)}
                        WHERE {nameof(Domain.Entities.Product.Name)} LIKE '%{request.SearchTerm}%'
                        OR {nameof(Domain.Entities.Product.Description)} LIKE '%{request.SearchTerm}%'
                        ORDER BY ";

                foreach (var item in request.SortColumnAndOrder)
                    productsQuery += item.Value == SortOrder.Descending
                        ? $"{item.Key} DESC, "
                        : $"{item.Key} ASC, ";

                productsQuery = productsQuery.Remove(productsQuery.Length - 2);

                productsQuery += $" OFFSET {(PageIndex - 1) * PageSize} ROWS FETCH NEXT {PageSize} ROWS ONLY";

                var products = await _context.Products.FromSqlRaw(productsQuery)
                    .ToListAsync(cancellationToken: cancellationToken);

                var totalCount = await _context.Products.CountAsync(cancellationToken);

                var productPagedResult = PagedResult<Domain.Entities.Product>.Create(products,
                    PageIndex,
                    PageSize,
                    totalCount);

                var result = _mapper.Map<PagedResult<Response.ProductResponse>>(productPagedResult);

                return Result.Success(result);

            }
            else
            {
                var productsQuery = _productRepository.FindAll(a => string.IsNullOrEmpty(request.SearchTerm) || (string.IsNullOrEmpty(request.SearchTerm) && (a.Name.Contains(request.SearchTerm) || a.Description.Contains(request.SearchTerm))));
            //    var productsQuery = string.IsNullOrWhiteSpace(request.SearchTerm)
            //? _productRepository.FindAll()
            //: _productRepository.FindAll(x => x.Name.Contains(request.SearchTerm) || x.Description.Contains(request.SearchTerm));

                productsQuery = request.SortOrder == SortOrder.Descending
                ? productsQuery.OrderByDescending(GetSortProperty(request))
                : productsQuery.OrderBy(GetSortProperty(request));

                var products = await PagedResult<Domain.Entities.Product>.CreateAsync(productsQuery,
                    request.PageIndex,
                    request.PageSize);

                var result = _mapper.Map<PagedResult<Response.ProductResponse>>(products);
                return Result.Success(result);
            }
        }
    }
}

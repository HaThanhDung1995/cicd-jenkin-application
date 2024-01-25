using DemoCICD.Contract.Abstractions.Messages;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DemoCICD.Contract.Services.Products.Response;

namespace DemoCICD.Contract.Services.Products
{
    public static class Query
    {
        public record GetProductQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<Response.ProductResponse>>;
        public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;
    }
}

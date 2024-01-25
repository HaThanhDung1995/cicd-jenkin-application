using Asp.Versioning;
using DemoCICD.Contract.Abstractions.Shared;
using DemoCICD.Contract.Extensions;
using DemoCICD.Contract.Services.Products;
using DemoCICD.Contract.Shared;
using DemoCICD.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Presentation.Controllers.V1
{
    [ApiVersion(1)]
    public class ProductController : ApiController
    {
        private readonly ILogger<ProductController> _logger;
        public ProductController(ISender Sender, ILogger<ProductController> logger) : base(Sender) {
            _logger = logger;
        }
        [HttpPost(Name = "CreateProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Products([FromBody] Command.CreateProductCommand CreateProduct)
        {
            var result = await Sender.Send(CreateProduct);

            if (result.IsFailure)
                return HandlerFailure(result);

            return Ok(result);
        }
        [HttpGet(Name = "Get Products")]
        [ProducesResponseType(typeof(PagedResult<Response.ProductResponse>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Products(string? serchTerm = null,
        string? sortColumn = null,
        string? sortOrder = null,
        string? sortColumnAndOrder = null,
        int pageIndex = 1,
        int pageSize = 10)
        {
            var result = await Sender.Send(new Query.GetProductQuery(serchTerm, sortColumn,
            SortOrderExtension.ConvertStringToSortOrder(sortOrder),
            SortOrderExtension.ConvertStringToSortOrderV2(sortColumnAndOrder),
            pageIndex,
            pageSize));
            return Ok(result);
        }
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(Result<Response.ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Products(Guid productId)
        {
            var result = await Sender.Send(new Query.GetProductByIdQuery(productId));
            return Ok(result);
        }
        [HttpDelete("{productId}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProducts(Guid productId)
        {
            var result = await Sender.Send(new Command.DeleteProductCommand(productId));
            return Ok(result);
        }
        [HttpPut("{productId}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Products(Guid productId, [FromBody] Command.UpdateProductCommand updateProduct)
        {
            var updateProductCommand = new Command.UpdateProductCommand(productId, updateProduct.Name, updateProduct.Price, updateProduct.Description);
            var result = await Sender.Send(updateProductCommand);
            return Ok(result);
        }
    }
}

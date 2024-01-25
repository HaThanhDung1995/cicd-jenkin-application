using Asp.Versioning;
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

namespace DemoCICD.Presentation.Controllers.V2
{
    [ApiVersion(2)]
    public class ProductController : ApiController
    {
        private readonly ILogger<ProductController> _logger;
        public ProductController(ISender Sender, ILogger<ProductController> logger) : base(Sender)
        {
            _logger = logger;
        }

        //[HttpGet(Name = "Get Products")]
        //[ProducesResponseType(typeof(Result<IEnumerable<int>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Products()
        //{
        //    var result = await Sender.Send(new Query.GetProductQuery());
        //    return Ok(result);
        //}
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(Result<Response.ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Products(Guid productId)
        {

            return Ok();
        }
        //[HttpGet(Name = "{productId}")]
        //[ProducesResponseType(typeof(Result<IEnumerable<int>>), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Products(Guid productId)
        //{
        //    var result = await Sender.Send(new Query.GetProductQuery());
        //    return Ok(result);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Services.Products
{
    public static class Response
    {
        public record ProductResponse(Guid Id, string Name, decimal Price, string Description);
    }
}

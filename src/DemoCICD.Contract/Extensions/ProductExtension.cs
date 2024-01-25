using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Extensions
{
    public static class ProductExtension
    {
        public static string GetSortProductProperty(string sortColumn)
            => sortColumn.ToLower() switch
            {
                "name" => "Name",
                "price" => "Price",
                "description" => "Description",
                _ => "Id"
            };
    }
}

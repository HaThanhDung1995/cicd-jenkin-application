using DemoCICD.Domain.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Domain.Entities
{
    public class Product : DomainEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(19,4)")]
        public decimal Price { get; set; }
        public static Product CreateProduct(Guid id, string name, decimal price, string description)
        {
            return new Product(id, name, price, description);
        }

        public Product(Guid id, string name, decimal price, string description)
        {
            //if (!NameValidation(name))
            //    throw new ArgumentNullException();
            Id = id;
            Name = name;
            Price = price;
            Description = description;
        }
    }
}

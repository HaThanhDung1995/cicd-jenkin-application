using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Domain.Abstractions.Entities
{
    public abstract class DomainEntity<T>
    {
        public virtual T Id { get; set; }
        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }
    }
}

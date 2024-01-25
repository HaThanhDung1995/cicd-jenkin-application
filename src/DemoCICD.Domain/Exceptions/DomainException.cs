using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string title, string message)
            : base(message) =>
            Title = title;

        public string Title { get; }
    }
}

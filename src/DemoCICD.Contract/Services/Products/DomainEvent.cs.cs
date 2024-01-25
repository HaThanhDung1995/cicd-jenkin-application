using DemoCICD.Contract.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Services.Products
{
    public static class DomainEvent
    {
        public record ProductCreated(Guid Id) : IDomainEvent;
        public record ProductDeleted(Guid Id) : IDomainEvent;
        public record ProductUpdated(Guid Id) : IDomainEvent;
    }
}

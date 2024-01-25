using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Abstractions.Messages
{
    public interface IDomainEvent : INotification
    {
        public Guid Id { get; init; }
    }
}

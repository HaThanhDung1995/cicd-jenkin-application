using DemoCICD.Contract.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Abstractions.Messages
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}

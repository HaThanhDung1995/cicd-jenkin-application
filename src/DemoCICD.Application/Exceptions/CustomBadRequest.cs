using DemoCICD.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.Exceptions
{
    public class CustomBadRequest : BadRequestException
    {
        public CustomBadRequest() : base("Bad Request Hihih") { }
    }
}

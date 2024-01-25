using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Services.Products.Validators
{
    public class GetProductByIdValidator : AbstractValidator<Query.GetProductByIdQuery>
    {
        public GetProductByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

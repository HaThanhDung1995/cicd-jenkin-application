using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Contract.Services.Products.Validators
{
    public class UpdateProductValidator : AbstractValidator<Command.UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(a=>a.Id).NotNull().NotEmpty();
        }
    }
}

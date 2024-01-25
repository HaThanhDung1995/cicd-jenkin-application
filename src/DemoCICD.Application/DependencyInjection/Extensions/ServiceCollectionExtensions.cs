using DemoCICD.Application.Behaviors;
using DemoCICD.Application.Mappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigureMediart(this IServiceCollection services)
        {

            return services
                .AddMediatR(options => options.RegisterServicesFromAssembly(AssemblyReference.Assembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
                .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly,includeInternalTypes: true)
                ;
        }
        public static IServiceCollection AddConfigureAutoMapper(this IServiceCollection services)
        {

            return services
                .AddAutoMapper(typeof(ServiceProfile))
                ;
        }
    }
}

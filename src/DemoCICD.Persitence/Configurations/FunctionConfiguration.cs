using DemoCICD.Persitence.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Function = DemoCICD.Domain.Entities.Identity.Function;
namespace DemoCICD.Persitence.Configurations
{
    internal sealed class FunctionConfiguration : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.ToTable(TableNames.Functions);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(50);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(true);
            builder.Property(x => x.ParrentId)
                .HasMaxLength(50)
                .HasDefaultValue(null);
            builder.Property(x => x.CssClass).HasMaxLength(50).HasDefaultValue(null);
            builder.Property(x => x.Url).HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.SortOrder).HasDefaultValue(null);

            // Each User can have many Permission
            builder.HasMany(e => e.Permissions)
                .WithOne()
                .HasForeignKey(p => p.FunctionId)
                .IsRequired();

            // Each User can have many ActionInFunction
            builder.HasMany(e => e.ActionInFunctions)
                .WithOne()
                .HasForeignKey(aif => aif.FunctionId)
                .IsRequired();
        }
    }
}

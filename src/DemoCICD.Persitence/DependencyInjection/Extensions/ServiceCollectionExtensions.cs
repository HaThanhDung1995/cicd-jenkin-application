using DemoCICD.Domain.Abstractions;
using DemoCICD.Domain.Abstractions.Repositories;
using DemoCICD.Domain.Entities.Identity;
using DemoCICD.Persitence.DependencyInjection.Options;
using DemoCICD.Persitence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Persitence.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSqlConfiguration(this IServiceCollection services,Serilog.ILogger logger)
        {
            services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

                #region ============== SQL-SERVER-STRATEGY-1 ==============

                builder
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(false)
                .UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
                .ConfigureWarnings(a => a.Ignore(SqlServerEventId.DecimalTypeKeyWarning))
                .UseSqlServer(
                    connectionString: configuration.GetConnectionString("ConnectionStrings"),
                    sqlServerOptionsAction: optionsBuilder
                            => optionsBuilder.ExecutionStrategy(
                                    dependencies => new SqlServerRetryingExecutionStrategy(
                                        dependencies: dependencies,
                                        maxRetryCount: options.CurrentValue.MaxRetryCount,
                                        maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                        errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
                .LogTo(logger.Information,
                         new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
                ;

                #endregion ============== SQL-SERVER-STRATEGY-1 ==============

                #region ============== SQL-SERVER-STRATEGY-2 ==============

                //builder
                //.EnableDetailedErrors(true)
                //.EnableSensitiveDataLogging(true)
                //.UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
                //.UseSqlServer(
                //    connectionString: configuration.GetConnectionString("ConnectionStrings"),
                //        sqlServerOptionsAction: optionsBuilder
                //            => optionsBuilder
                //            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

                #endregion ============== SQL-SERVER-STRATEGY-2 ==============
            });

            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true; // Default true
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
                opt.Lockout.MaxFailedAccessAttempts = 3; // Default 5
            })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = true; // Default true
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); // Default 5
                options.Lockout.MaxFailedAccessAttempts = 3; // Default 5
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.AllowedForNewUsers = true;
            });
        }

        public static void AddRepositoryBaseConfiguration(this IServiceCollection services)
        {
            services.AddTransient(typeof(IUnitOfWork<>), typeof(EFUnitOfWork<>));
            services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

        }

        public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptions(this IServiceCollection services, IConfigurationSection section)
            => services
                .AddOptions<SqlServerRetryOptions>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
    }
}

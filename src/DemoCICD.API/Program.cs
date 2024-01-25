using DemoCICD.Application.DependencyInjection.Extensions;
using DemoCICD.Persitence.DependencyInjection.Extensions;
using DemoCICD.Persitence.DependencyInjection.Options;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using DemoCICD.API.DependencyInjection.Extensions;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SeriLogThemesLibrary;
using DemoCICD.API.Middleware;


var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json")
                      .Build();
// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(configuration)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", theme: SeriLogCustomThemes.Theme1())
    .CreateLogger();
var data = Log.Logger;
builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



//Configure services
builder.Services.AddConfigureMediart();
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlConfiguration(data);
builder.Services.AddConfigureAutoMapper();
builder.Services.AddRepositoryBaseConfiguration();
builder
    .Services
    .AddControllers()
    .AddApplicationPart(DemoCICD.Presentation.AssemblyReference.Assembly);
//builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();


builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.ConfigureSwagger();
Log.Logger.Information("Start.........");
app.UseMiddleware<ExceptionHandlingMiddleware>();
try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

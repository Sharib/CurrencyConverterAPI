using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Infrastructure.Configuration;
using CurrencyConverterAPI.Infrastructure.Repositories;
using CurrencyConverterAPI.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddTransient<ICurrencyConverterClient, CurrencyConverterClient>();
        
        // Configure MongoDB settings
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        
        // Use MongoDB repository instead of in-memory
        services.AddScoped<IMaintenanceIncidentRepository, MongoDbMaintenanceIncidentRepository>();

        return services;
    }
}
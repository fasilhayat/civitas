namespace Civitas.Api;

using Application.Services;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.OpenApi.Models;

/// <summary>
/// Provides extension methods for configuring services in the application's dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the services needed by the application, including DbContext, repositories, and service classes.
    /// </summary>
    /// <param name="services">The collection of services to register.</param>
    /// <param name="configuration">The configuration settings used to configure services.</param>
    /// <returns>The updated service collection with the added services.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext registration for dependency injection
        //services.AddScoped(_ => new DbContext(configuration.GetConnectionString("CivitasConnection")));

        // Register repositories and service classes for dependency injection
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<EmployeeService>();

        services.AddScoped<ISalaryRepository, SalaryRepository>();
        services.AddScoped<SalaryService>();

        return services;
    }

    /// <summary>
    /// Configures Swagger to document the API and adds security settings for API key validation.
    /// </summary>
    /// <param name="services">The collection of services to register.</param>
    /// <param name="configuration">The configuration settings used to configure Swagger.</param>
    /// <returns>The updated service collection with the added Swagger configuration.</returns>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = configuration.GetSection("ApiSettings");
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Define Swagger document details
            options.SwaggerDoc("v1", new()
            {
                Version = "v1",
                Title = apiSettings.GetValue<string>("Title"),
                Description = apiSettings.GetValue<string>("Description")!.Replace("\n", "<br>"),
                TermsOfService = new(apiSettings.GetValue<string>("TermsOfService")!),
                Contact = new()
                {
                    Name = apiSettings.GetSection("Contact").GetValue<string>("Name"),
                    Url = new(apiSettings.GetSection("Contact").GetValue<string>("Url")!),
                    Email = apiSettings.GetSection("Contact").GetValue<string>("Email")
                }
            });

            // Configure API key security definition
            options.AddSecurityDefinition("ApiKey", new()
            {
                Name = "X-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "Enter your API key to access this API."
            });

            // Add security requirement for API key
            options.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}

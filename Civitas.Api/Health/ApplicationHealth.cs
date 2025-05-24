namespace Civitas.Api.Health;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

/// <summary>
/// Enables common Integration capabilities for Health
/// </summary>
public static class ApplicationHealth
{
    /// <summary>
    /// Add Health to an Application, to expose Health endpoint.
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/> used for build</param>
    /// <returns><see cref="WebApplicationBuilder"/> used for build</returns>
    /// <example>
    /// <code>
    /// How to use:
    /// var builder = WebApplication.CreateBuilder(args);
    /// var app = builder.AddAddHealth().Build();
    /// app.UseHealth();
    /// app.Run();
    /// </code>
    /// </example>
    public static WebApplicationBuilder AddHealth(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealth();
        return builder;
    }

    /// <summary>
    /// Add standard Health services part of a WebApplication.
    /// This will standardize how the service report Health.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> used in add</param>
    /// <returns><see cref="IServiceCollection"/> with added HealthCheck</returns>
    public static IServiceCollection AddHealth(this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }

    /// <summary>
    /// Use standard Health.
    /// </summary>
    /// <param name="app"><see cref="IEndpointRouteBuilder"/> used for build</param>
    /// <returns><see cref="IEndpointRouteBuilder"/> used for build</returns>
    /// <remarks>Used in combination with <see cref="AddHealth(WebApplicationBuilder)"/> or <see cref="AddHealth(IServiceCollection)"/></remarks>
    public static IEndpointRouteBuilder UseHealth(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/healthz", new()
        {
            Predicate = _ => true,
            ResponseWriter = HealthWriter.WriteHealthUiResponse
        }).WithMetadata(new AllowAnonymousAttribute());

        return app;
    }

    /// <summary>
    /// Create a HealthBuilder from a service collection.
    /// This will standardize how to build service Health checks.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> used in create</param>
    /// <returns><see cref="IHealthChecksBuilder"/> with added standard Check</returns>
    public static IHealthChecksBuilder CreateHealthChecksBuilder(this IServiceCollection services)
    {
        var healthChecksBuilder = services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
        return healthChecksBuilder;
    }

    /// <summary>
    /// Add standard Url HealthChecks as part of a builder pattern.
    /// </summary>
    /// <param name="builder"><see cref="IHealthChecksBuilder"/> used in add</param>
    /// <param name="url"> url to external endpoint</param>
    /// <param name="tag"> name to filter endpoints</param>
    /// <returns><see cref="IHealthChecksBuilder"/> with added HealthChecks</returns>
    public static IHealthChecksBuilder AddUrlHealthCheck(this IHealthChecksBuilder builder, Uri url, string tag)
    {
        builder.AddUrlGroup( // This method is part of the HealthChecks.Network package
            uri: url,
            name: tag,
            tags: new[] { tag },
            failureStatus: HealthStatus.Unhealthy,
            timeout: new TimeSpan(0, 0, 0, 3)
        );

        return builder;
    }
}
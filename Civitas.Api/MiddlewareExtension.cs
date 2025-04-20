namespace Civitas.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Provides extension methods for configuring middleware in the application's request pipeline.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Configures the middleware pipeline, including API key validation and Swagger setup for the development environment.
    /// </summary>
    /// <param name="app">The application builder to configure the middleware.</param>
    /// <param name="environment">The hosting environment that the application is running in.</param>
    /// <param name="configuration">The configuration settings used for middleware setup.</param>
    /// <returns>The updated application builder with the configured middleware.</returns>
    public static void UseMiddlewareConfiguration(this IApplicationBuilder app, IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        // Add API key middleware to the pipeline
        app.UseApiKeyMiddleware(configuration);

        // If in the development environment, add Swagger-related middleware
        if (environment.IsDevelopment())
            UseSwaggerOptions(app);
    }

    /// <summary>
    /// Configures and enables Swagger UI for API documentation in the development environment.
    /// </summary>
    /// <param name="app">The application builder to configure the Swagger middleware.</param>
    private static void UseSwaggerOptions(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseStaticFiles(); // Serve static files such as CSS and JS for Swagger UI
        app.UseSwaggerUI(options =>
        {
            // Customize Swagger UI with dark theme and custom JavaScript
            options.InjectStylesheet("/swagger-ui/darktheme.css");
            options.InjectJavascript("/swagger-ui/custom.js");
            options.RoutePrefix = "swagger"; // Set the Swagger UI endpoint prefix
            options.DocumentTitle = "Civitas API Documentation";
        });
    }

    /// <summary>
    /// Adds the API key validation middleware to the application's request pipeline.
    /// </summary>
    /// <param name="app">The application builder to configure the middleware.</param>
    /// <param name="configuration">The configuration containing the API key to validate.</param>
    private static void UseApiKeyMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        // Use the ApiKeyMiddleware with the provided configuration
        app.UseMiddleware<ApiKeyMiddleware>(configuration);
    }
}
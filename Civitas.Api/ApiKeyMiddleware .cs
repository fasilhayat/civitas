namespace Civitas.Api;

/// <summary>
/// Middleware to enforce API key authentication for incoming HTTP requests.
/// </summary>
public class ApiKeyMiddleware
{
    /// <summary>
    /// Delegate representing the next middleware in the pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// The name of the HTTP header expected to contain the API key.
    /// </summary>
    private const string ApiKeyHeaderName = "X-API-KEY";

    /// <summary>
    /// The API key configured on the server, retrieved from configuration settings.
    /// </summary>
    private readonly string? _configuredApiKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="configuration">The configuration used to retrieve the API key.</param>
    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuredApiKey = configuration["API_KEY"]; // Retrieve the API key from environment variables
    }

    /// <summary>
    /// Processes an incoming HTTP request to validate the API key.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Bypass the middleware for Swagger UI requests
        if (context.Request.Path.StartsWithSegments("/swagger") || context.Request.Path.StartsWithSegments("/swagger-ui"))
        {
            await _next(context);
            return;
        }

        // Check if the API key is configured
        if (string.IsNullOrEmpty(_configuredApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "API key is not configured on the server." });
            return;
        }

        // Validate the API key for other requests
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey) || extractedApiKey != _configuredApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = "Unauthorized: Invalid or missing API key." });
            return;
        }

        // Call the next middleware if the API key is valid
        await _next(context);
    }
}
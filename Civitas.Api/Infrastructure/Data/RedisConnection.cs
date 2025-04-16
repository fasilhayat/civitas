namespace Civitas.Api.Infrastructure.Data;

/// <summary>
/// Represents a connection configuration for Redis.
/// </summary>
public class RedisConnection
{
    /// <summary>
    /// The Redis connection string.
    /// </summary>
    public string? Host { get; init; }

    /// <summary>
    /// The Redis connection port.
    /// </summary>
    public string? Port { get; init; }

    /// <summary>
    /// Flag indicating whether to use SSL for the connection.
    /// </summary>
    public bool IsSsl { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? Password { get; init; }
}


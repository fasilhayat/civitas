namespace Civitas.Api.Infrastructure.Data;

/// <summary>
/// Represents a connection configuration for Redis.
/// </summary>
public class RedisConnection
{
    public string? Host { get; init; }

    public string? Port { get; init; }

    public bool IsSsl { get; init; }

    public string? Password { get; init; }
}


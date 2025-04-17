namespace Civitas.Api.Infrastructure.Data;

using Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

/// <summary>
/// Represents a database context for data access.
/// </summary>
public class DbContext : IDbContext
{
    /// <summary>
    /// The Redis database used for data access.
    /// </summary>
    private readonly IDatabase _redisDb;

    /// <summary>
    /// The default time-to-live (TTL) for cached items in the database.
    /// </summary>
    private readonly TimeSpan _defaultTtl;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbContext"/> class.
    /// </summary>
    /// <param name="redis"> The Redis connection multiplexer used to access the database.</param>
    /// <param name="configuration"></param>
    public DbContext(IConnectionMultiplexer redis, IConfiguration configuration)
    {
        _redisDb = redis.GetDatabase();

        // Read TTL from config (in minutes)
        var ttlMinutes = configuration.GetValue<int>("Redis:DefaultTtlMinutes", 5000);
        _defaultTtl = TimeSpan.FromMinutes(ttlMinutes);
    }

    /// <summary>
    /// Retrieves a value from the database using the specified key.
    /// </summary>
    /// <param name="key">The key used to access the value.</param>
    /// <returns>Returns the value associated with the key, or null if not found.</returns>
    public async Task Delete(IDataKey key)
    {
        await _redisDb.KeyDeleteAsync(key.Identifier);
    }

    /// <summary>
    /// Retrieves a value from the database using the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="dataKey">The key used to access the value.</param>
    /// <returns>The value associated with the key, or null if not found.</returns>
    public async Task<T?> GetData<T>(IDataKey dataKey) where T : class
    {
        var value = await _redisDb.StringGetAsync(dataKey.Identifier);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : null;
    }

    /// <summary>
    /// Retrieves a value from the database using the specified key and type.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key used to access the value.</param>
    /// <returns>The value associated with the key, or null if not found.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<T?> GetHashData<T>(IDataKey key) where T : class
    {
        var typeField = typeof(T).FullName;
        if (string.IsNullOrEmpty(typeField))
            throw new InvalidOperationException("Could not determine the type name for the hash field.");

        var value = await _redisDb.HashGetAsync(key.Identifier, typeField);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : null;
    }

    /// <summary>
    /// Saves a value to the database using the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to save.</typeparam>
    /// <param name="key">The key used to access the value.</param>
    /// <param name="obj">The object to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveData<T>(IDataKey key, T obj) where T : class
    {
        var serialized = JsonSerializer.Serialize(obj);
        await _redisDb.StringSetAsync(key.Identifier, serialized, _defaultTtl);
    }


    /// <summary>
    /// Saves a value to the database using the specified key and type.
    /// </summary>
    /// <param name="key">The key used to access the value.</param>
    public async Task ClearData(IDataKey key)
    {
        await _redisDb.KeyDeleteAsync(key.Identifier);
    }
}
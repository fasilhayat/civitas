namespace Civitas.Api.Infrastructure.Data;

using Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

/// <summary>
/// Represents a database context for data access.
/// </summary>
public class DbContext : IDbContext
{
    /// <summary>
    /// The distributed cache used for data storage.
    /// </summary>
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbContext"/> class with the specified distributed cache.
    /// </summary>
    /// <param name="cache"></param>
    public DbContext(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Deletes the data associated with the specified key.
    /// </summary>
    /// <param name="key">The key to identify the data to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Delete(IDataKey key)
    {
        await ClearData(key);
    }

    /// <summary>
    /// Retrieves the data associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the data to be retrieved.</typeparam>
    /// <param name="dataKey">The key to identify the data.</param>
    /// <returns>Returns the data of type <typeparamref name="T"/> if found; otherwise, null.</returns>
    public async Task<T?> GetData<T>(IDataKey dataKey) where T : class
    {
        var document = await _cache.GetAsync(dataKey.Identifier);
        return document != null ? JsonSerializer.Deserialize<T>(document) : null;
    }

    /// <summary>
    /// Saves the specified data associated with the given key.
    /// </summary>
    /// <typeparam name="T">The type of the data to be saved.</typeparam>
    /// <param name="key">The key to identify the data.</param>
    /// <param name="o">The data to be saved.</param>
    /// <returns>Returns the saved data of type <typeparamref name="T"/>.</returns>
    public async Task<T> SaveData<T>(IDataKey key, T o) where T : class
    {
        var cachePolicy = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5000),
        };
        var serializedData = JsonSerializer.SerializeToUtf8Bytes(o);
        await _cache.SetAsync(key.Identifier, serializedData, cachePolicy);
        return o;
    }

    /// <summary>
    /// Clears the data associated with the specified key.
    /// </summary>
    /// <param name="key">The key to identify the data to be cleared.</param>
    /// <returns>Clears the data associated with the specified key.</returns>
    public async Task ClearData(IDataKey key)
    {
        await _cache.RemoveAsync(key.Identifier);
    }
}

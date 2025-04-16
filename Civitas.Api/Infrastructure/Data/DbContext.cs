namespace Civitas.Api.Infrastructure.Data;

using Core.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class DbContext : IDbContext
{
    private readonly IDistributedCache _cache;

    public DbContext(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task Delete(IDataKey key)
    {
        await ClearData(key);
    }

    public async Task<T?> GetData<T>(IDataKey dataKey) where T : class
    {
        var document = await _cache.GetAsync(dataKey.Identifier);
        return document != null ? JsonSerializer.Deserialize<T>(document) : null;
    }

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

    public async Task ClearData(IDataKey key)
    {
        await _cache.RemoveAsync(key.Identifier);
    }
}

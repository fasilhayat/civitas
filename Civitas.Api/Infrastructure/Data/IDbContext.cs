namespace Civitas.Api.Infrastructure.Data;

using Core.Interfaces;

/// <summary>
/// Represents a database context for data access.
/// </summary>
public interface IDbContext 
{
    Task<T?> GetData<T>(IDataKey key) where T : class;

    Task SaveData<T>(IDataKey key, T obj) where T : class;

    Task ClearData(IDataKey key);

    Task Delete(IDataKey key);
}
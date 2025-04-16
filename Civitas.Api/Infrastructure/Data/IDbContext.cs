using Civitas.Api.Core.Interfaces;

namespace Civitas.Api.Infrastructure.Data;

public interface IDbContext 
{
    Task<T?> GetData<T>(IDataKey key) where T : class;

    Task<T> SaveData<T>(IDataKey key, T o) where T : class;

    Task ClearData(IDataKey key);

    Task Delete(IDataKey key);
}
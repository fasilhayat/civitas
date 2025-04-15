namespace Civitas.Api.Infrastructure.Data;

public interface IDbContext : IDeletionPolicy
{
    T? GetData<T>(IDataKey key) where T : class;

    T SaveData<T>(IDataKey key, T o) where T : class;

    void ClearData(IDataKey key);

    void ClearAllData();
}
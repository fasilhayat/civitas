namespace Civitas.Api.Infrastructure.Data;

/// <summary>
/// 
/// </summary>
public interface IDeletionPolicy
{
    public void Delete(IDataKey key);
}


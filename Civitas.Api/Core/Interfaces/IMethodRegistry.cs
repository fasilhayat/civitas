namespace Civitas.Api.Core.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IMethodRegistry
{
    /// <summary>
    /// Registers a method handler with a specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Func<object?, Task<bool>> GetHandler(string key);
}



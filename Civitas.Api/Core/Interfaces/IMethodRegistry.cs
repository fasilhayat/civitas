namespace Civitas.Api.Core.Interfaces;

/// <summary>
/// Generic interface for method registry.
/// </summary>
public interface IMethodRegistry
{
    /// <summary>
    /// Registers a method handler with a specified key.
    /// </summary>
    /// <param name="key">The key to register the handler under.</param>
    /// <param name="handler">The method handler to register.</param>
    void Register(string key, Func<object?, Task<bool>> handler);

    /// <summary>
    /// Registers a method handler with a specified key.
    /// </summary>
    /// <param name="key">The key to register the handler under.</param>
    /// <returns>The method handler associated with the specified key.</returns>
    Func<object?, Task<bool>> GetHandler(string key);
}
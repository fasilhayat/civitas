namespace Civitas.Api.Infrastructure.Repositories;

using Core.Interfaces;

/// <summary>
/// Registry for method handlers.
/// </summary>
public class MethodRegistry : IMethodRegistry
{
    /// <summary>
    /// Dictionary to store method handlers with their keys.
    /// </summary>
    private readonly Dictionary<string, Func<object?, Task<bool>>> _handlers = new();

    /// <summary>
    /// Registers a method handler with a specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="handler"></param>
    public void Register(string key, Func<object?, Task<bool>> handler)
    {
        _handlers[key] = handler;
    }

    /// <summary>
    /// Gets the handler for a specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Func<object?, Task<bool>> GetHandler(string key)
    {
        if (!_handlers.TryGetValue(key, out var handler))
            throw new InvalidOperationException($"No handler registered for method key: {key}");

        return handler;
    }
}
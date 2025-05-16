namespace Civitas.Api.Infrastructure.Repositories;

using Core.Interfaces;

/// <summary>
/// Registry for method handlers.
/// </summary>
public class MethodRegistry : IMethodRegistry
{
    private readonly Dictionary<string, Func<object?, Task<bool>>> _handlers = new();

    public void Register(string key, Func<object?, Task<bool>> handler)
    {
        _handlers[key] = handler;
    }

    public Func<object?, Task<bool>> GetHandler(string key)
    {
        if (!_handlers.TryGetValue(key, out var handler))
            throw new InvalidOperationException($"No handler registered for method key: {key}");

        return handler;
    }
}
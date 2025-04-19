namespace Civitas.Api.Core.Entities;

using Interfaces;

/// <summary>
/// Represents a key for data access.
/// </summary>
public class DataKey : IDataKey
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataKey"/> class with the specified identifier.
    /// </summary>
    /// <param name="identifier"></param>
    public DataKey(string identifier)
    {
        Identifier = identifier;
    }

    /// <summary>
    /// Gets the identifier for the data key.
    /// </summary>
    public string Identifier { get; init; }
}
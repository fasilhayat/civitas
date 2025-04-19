namespace Civitas.MessageProcessor.Core.Interfaces;

/// <summary>
/// Represents a key for data access.
/// </summary>
public interface IDataKey
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IDataKey"/> class with the specified identifier.
    /// </summary>
    string Identifier { get; init; }
}
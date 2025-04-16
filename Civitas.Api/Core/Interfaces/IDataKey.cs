namespace Civitas.Api.Core.Interfaces;

using Entities;

/// <summary>
/// Represents a key for data access.
/// </summary>
public interface IDataKey
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataKey"/> class with the specified identifier.
    /// </summary>
    string Identifier { get; init; }
}
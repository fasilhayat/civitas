namespace Civitas.Api.Infrastructure.Data;

using Core.Interfaces;

/// <summary>
/// Represents a database context for data access.
/// </summary>
public interface IDbContext 
{
    /// <summary>
    /// Retrieves a value from the database using the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key used to access the value.</param>
    /// <returns>The value associated with the key, or null if not found.</returns>
    Task<T?> GetData<T>(IDataKey key) where T : class;

    /// <summary>
    /// Retrieves a value from the database using the specified key and type.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key used to access the value.</param>
    /// <returns>The value associated with the key, or null if not found.</returns>
    Task<T?> GetHashData<T>(IDataKey key) where T : class;

    /// <summary>
    /// Saves a value to the database using the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the value to save.</typeparam>
    /// <param name="key">The key used to access the value.</param>
    /// <param name="obj">The object to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveData<T>(IDataKey key, T obj) where T : class;

    /// <summary>
    /// Clears the value to the database using the specified key and type.
    /// </summary>
    /// <param name="key">The key used to access the value.</param>
    /// <returns></returns>
    Task ClearData(IDataKey key);

    /// <summary>
    /// Deletes the value from the database using the specified key.
    /// </summary>
    /// <param name="key">The key used to find the value.</param>
    /// <returns>Returns the value associated with the key, or null if not found.</returns>
    Task Delete(IDataKey key);
}
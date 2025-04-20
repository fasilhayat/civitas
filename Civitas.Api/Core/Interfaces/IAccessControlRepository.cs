namespace Civitas.Api.Core.Interfaces;

using Entities;

/// <summary>
/// Interface for Access Control Repository.
/// </summary>
public interface IAccessControlRepository
{
    /// <summary>
    /// Get access control list asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns>Returns a list of access roles for the employee.</returns>
    Task<AccessLevelControl> GetEmployeeAccessControl(long identity);
}
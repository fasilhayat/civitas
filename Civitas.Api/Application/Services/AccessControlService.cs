namespace Civitas.Api.Application.Services;

using Core.Entities;
using Core.Interfaces;

/// <summary>
/// Service class for managing access control.
/// </summary>
public class AccessControlService
{
    /// <summary>
    /// The repository used to access access control data.
    /// </summary>
    private readonly IAccessControlRepository _accessControlRepository;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="accessControlRepository">The access control repository.</param>
    public AccessControlService(IAccessControlRepository accessControlRepository)
    {
        _accessControlRepository = accessControlRepository;
    }

    /// <summary>
    /// Get employee access control asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns>Returns the access control for the employee</returns>
    public async Task<AccessLevelControl> GetEmployeeAccessControl(long identity)
    {
        return await _accessControlRepository.GetEmployeeAccessControl(identity);
    }
}
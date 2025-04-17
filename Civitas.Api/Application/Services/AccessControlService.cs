namespace Civitas.Api.Application.Services;

using Core.Entities;
using Core.Interfaces;

/// <summary>
/// Service class for managing access control.
/// </summary>
public class AccessControlService
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IAccessControlRepository _accessControlRepository;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="accessControlRepository"></param>
    public AccessControlService(IAccessControlRepository accessControlRepository)
    {
        _accessControlRepository = accessControlRepository;
    }

    /// <summary>
    /// Get employee access control asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns></returns>
    public async Task<AccessLevelControl> GetEmployeeAccessControl(long identity)
    {
        return await _accessControlRepository.GetEmployeeAccessControl(identity);
    }
}

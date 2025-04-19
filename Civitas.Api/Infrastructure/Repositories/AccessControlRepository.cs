namespace Civitas.Api.Infrastructure.Repositories;

using Core.Entities;
using Core.Interfaces;
using Data;

/// <summary>
/// Repository for managing access control data.
/// </summary>
public class AccessControlRepository : IAccessControlRepository
{
    /// <summary>
    /// The database context used for data access.
    /// </summary>
    private readonly IDbContext _context;

    /// <summary>
    /// The constructor for the AccessControlRepository class.
    /// </summary>
    /// <param name="context">The context object.</param>
    public AccessControlRepository(IDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Get access control list asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns>Returns a list of access roles for the employee.</returns>
    /// <exception cref="NotImplementedException">Throws not implemented exception.</exception>
    public Task<AccessLevelControl> GetEmployeeAccessControl(long identity)
    {
        // You can later use the 'identity' parameter to filter/lookup if needed
        var accessControl = new AccessLevelControl
        {
            Id = 3521,
            Name = "Default",
            IdentityKey = identity,
            DepartmentId = 18958,
            DepartmentName = "ADM - Direktion & Stab",
            Roles = new List<AccessRole>
            {
                new AccessRole
                {
                    Id = 17,
                    Name = "Forca Partner - Afdelingsleder",
                    ThirdPartyId = string.Empty,
                    AccessLevelControlId = 3521
                }
            }
        };

        return Task.FromResult(accessControl);
    }
}
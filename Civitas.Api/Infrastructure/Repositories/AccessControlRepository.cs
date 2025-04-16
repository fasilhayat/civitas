namespace Civitas.Api.Infrastructure.Repositories;

using Core.Entities;
using Core.Interfaces;
using Data;

public class AccessControlRepository : IAccessControlRepository
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IDbContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public AccessControlRepository(IDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Get access control list asynchronously.
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
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
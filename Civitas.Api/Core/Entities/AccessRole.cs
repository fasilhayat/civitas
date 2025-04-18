namespace Civitas.Api.Core.Entities;

/// <summary>
/// Represents an access role for an employee.
/// </summary>
public class AccessRole
{
    /// <summary>
    /// Unique identifier for the access role.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ThirdPartyId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int AccessLevelControlId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public AccessLevelControl? AccessLevelControl { get; set; }
}

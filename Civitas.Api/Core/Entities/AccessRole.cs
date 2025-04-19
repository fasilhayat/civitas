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
    /// The name of the access role.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The id of the third party system that the employee is associated with.
    /// </summary>
    public string? ThirdPartyId { get; set; }

    /// <summary>
    /// The id of the access level control that this role is associated with.
    /// </summary>
    public int AccessLevelControlId { get; set; }

    /// <summary>
    /// Access level control associated with this role.
    /// </summary>
    public AccessLevelControl? AccessLevelControl { get; set; }
}

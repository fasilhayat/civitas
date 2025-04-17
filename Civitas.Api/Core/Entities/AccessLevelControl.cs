namespace Civitas.Api.Core.Entities;

/// <summary>
/// Represents the access level control for an employee.
/// </summary>
public class AccessLevelControl
{
    /// <summary>
    /// Unique identifier for the access level control.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identity key of the employee.
    /// </summary>
    public long IdentityKey { get; set; }

    /// <summary>
    /// The unique identifier for the department.
    /// </summary>
    public int DepartmentId { get; set; }

    /// <summary>
    /// The name of the access level control.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Department name of the employee.
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// Access level of the employee.
    /// </summary>
    public List<AccessRole> Roles { get; set; } = new();
}

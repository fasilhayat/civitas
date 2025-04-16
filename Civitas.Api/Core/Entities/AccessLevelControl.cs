namespace Civitas.Api.Core.Entities;

public class AccessLevelControl
{
    public int Id { get; set; }

    /// <summary>
    /// Identity key of the employee.
    /// </summary>
    public long IdentityKey { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int DepartmentId { get; set; }

    /// <summary>
    /// 
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

namespace Civitas.Api.Core.Entities;

/// <summary>
/// Represents a employee entity with details about the employee.
/// </summary>
public class Employee
{
    /// <summary>
    /// Gets the unique identifier for the employee.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the first name of an emmployee.
    /// </summary>
    public string? FirstName { get; init; }

    /// <summary>
    /// Gets the middle name of an employee.
    /// </summary>
    public string? MiddleName { get; init; }

    /// <summary>
    /// Gets the last name of an employee.
    /// </summary>
    public string? LastName { get; init; }
}
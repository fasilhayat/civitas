namespace Civitas.Api.Core.Entities;

/// <summary>
/// Represents a salary entity with details about the employee's salary.
/// </summary>
public class Salary
{
    /// <summary>
    /// Gets the unique identifier for the employee.
    /// </summary>
    public Employee Employee { get; init; } = default!;

    /// <summary>
    /// Gets the first name of an employee.
    /// </summary>
    public string EmploymentId { get; init; } = default!;

    /// <summary>
    /// The date when the employee is available to start working.
    /// </summary>
    public DateTime? AvailabilityDate { get; init; }

    /// <summary>
    /// The date when the employee was hired or started working.
    /// </summary>
    public DateTime EmploymentDate { get; init; }

    /// <summary>
    /// The date when the employee's contract ends.
    /// </summary>
    public int ProbationPeriodId { get; init; }

    /// <summary>
    /// The location where the employee works.
    /// </summary>
    public string WorkLocation { get; init; } = default!;

    /// <summary>
    /// The date when the employee's seniority is calculated from.
    /// </summary>
    public DateTime SeniorityDate { get; init; }

    /// <summary>
    /// The id of the employee's job title.
    /// </summary>
    public int JobTitleId { get; init; }

    /// <summary>
    /// The id of the employee's job type.
    /// </summary>
    public int JobTypeId { get; init; }

    /// <summary>
    /// The amount of salary.
    /// </summary>
    public string SalaryAmount { get; init; } = default!; // kept as string per JSON

    /// <summary>
    /// The currency of the salary amount.
    /// </summary>
    public string CostCenter { get; init; } = default!;

    /// <summary>
    /// The category of the employee's staff.
    /// </summary>
    public string StaffCategory { get; init; } = default!;

    /// <summary>
    /// The percentage of the employee's work hours compared to a full-time equivalent (FTE).
    /// </summary>
    public int WorkPercentage { get; init; }

    /// <summary>
    /// The full-time equivalent (FTE) of the employee's work hours.
    /// </summary>
    public float Fte { get; init; }

    /// <summary>
    /// The number of hours the employee works per week.
    /// </summary>
    public WorkingHour WorkingHour { get; init; } = new();
}
namespace Civitas.Api.Core.Interfaces;

using Entities;

/// <summary>
/// This interface defines the contract for accessing and managing salary data.
/// </summary>
public interface ISalaryRepository
{
    /// <summary>
    /// Gets the salary information for a specific employee asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee.</param>
    /// <returns>Salary object containing the salary details of the employee.</returns>
    Task<Salary> GetEmployeeSalaryAsync(long identity);
}
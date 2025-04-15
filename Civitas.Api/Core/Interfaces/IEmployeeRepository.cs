namespace Civitas.Api.Core.Interfaces;

using Entities;

/// <summary>
/// Interface for accessing and managing employee (owner customers) data.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Retrieves all employees asynchronously from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the result being a collection of employees, or null if no data is found.</returns>
    Task<IEnumerable<Employee>> GetEmployeesAsync();

    /// <summary>
    /// Retrieves a specific employee asynchronously based on the provided identity.
    /// </summary>
    /// <param name="identity"></param>
    /// <returns> Returns a task representing the asynchronous operation, with the result being the employee object if found, or null if not found.</returns>
    Task<Employee> GetEmployeeAsync(long identity);

    /// <summary>
    /// Retrieves the total number of employee asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the result being the total count of employee.</returns>
    Task<int> GetNumberOfEmployeesAsync();
}
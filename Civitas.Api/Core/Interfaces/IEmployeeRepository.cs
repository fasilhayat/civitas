using Civitas.Api.Core.Entities;

namespace Civitas.Api.Core.Interfaces;

/// <summary>
/// Interface for accessing and managing employee (owner customers) data.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Retrieves all employees asynchronously from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the result being a collection of employees, or null if no data is found.</returns>
    Task<IEnumerable<Employee>?> EmployeesAsync();

    /// <summary>
    /// Retrieves the total number of employee asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with the result being the total count of employee.</returns>
    Task<int> NumberOfEmployeeAsync();

}
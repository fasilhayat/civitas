namespace Civitas.Api.Application.Services;

using Core.Entities;
using Core.Interfaces;

/// <summary>
/// The EmployeeService class provides methods to manage employee data.
/// </summary>
public class EmployeeService
{
    /// <summary>
    /// The repository used to access employee data.
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="employeeRepository">The employee repository.</param>
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// Get number of employees asynchronously.
    /// </summary>
    /// <returns>Returns the full list of employees</returns>
    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _employeeRepository.GetEmployeesAsync();
    }

    /// <summary>
    /// Get employee asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns>Returnsns the employee based on the identity</returns>
    public async Task<Employee?> GetEmployeeAsync(long identity)
    {
        return await _employeeRepository.GetEmployeeAsync(identity);
    }
    /// <summary>
    /// Get number of employees asynchronously.
    /// </summary>
    /// <returns>Returns the number of employees</returns>
    public async Task<int?> GetNumberOfEmployeesAsync()
    {
        return await _employeeRepository.GetNumberOfEmployeesAsync();
    }
}
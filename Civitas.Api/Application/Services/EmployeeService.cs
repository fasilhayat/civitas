namespace Civitas.Api.Application.Services;

using Core.Entities;
using Core.Interfaces;

/// <summary>
/// 
/// </summary>
public class EmployeeService
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="employeeRepository"></param>
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// Get number of employees asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _employeeRepository.GetEmployeesAsync();
    }

    /// <summary>
    /// Get employee asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns></returns>
    public async Task<Employee?> GetEmployeeAsync(long identity)
    {
        return await _employeeRepository.GetEmployeeAsync(identity);
    }
    /// <summary>
    /// Get number of employees asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task<int?> GetNumberOfEmployeesAsync()
    {
        return await _employeeRepository.GetNumberOfEmployeesAsync();
    }
}
namespace Civitas.Api.Application.Services;

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
    public async Task<int?> NumberOfEmployeesAsync()
    {
        return await _employeeRepository.NumberOfEmployeeAsync();
    }
}
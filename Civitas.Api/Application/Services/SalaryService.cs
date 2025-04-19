namespace Civitas.Api.Application.Services;

using Core.Entities;
using Core.Interfaces;

/// <summary>
/// The salary service class.
/// </summary>
public class SalaryService
{
    /// <summary>
    /// The salary repository.
    /// </summary>
    private readonly ISalaryRepository _salaryRepository;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="salaryRepository"></param>
    public SalaryService(ISalaryRepository salaryRepository)
    {
        _salaryRepository = salaryRepository;
    }

    /// <summary>
    /// Get employee asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns>Returns the employee salary based on the identity</returns>
    public async Task<Salary> GetEmployeeSalaryAsync(long identity)
    {
        return await _salaryRepository.GetEmployeeSalaryAsync(identity);
    }
}
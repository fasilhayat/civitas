namespace Civitas.Api.Core.Interfaces;

using Entities;

/// <summary>
/// 
/// </summary>
public interface ISalaryRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    Task<Salary> GetEmployeeSalaryAsync(long identity);
}


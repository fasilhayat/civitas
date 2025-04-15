namespace Civitas.Api.Infrastructure.Repositories;

using Core.Entities;
using Core.Interfaces;
using Data;

/// <summary>
/// 
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    /// <summary>
    /// 
    /// </summary>
    private readonly DbContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public EmployeeRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }


    public Task<IEnumerable<Employee>?> EmployeesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> NumberOfEmployeeAsync()
    {
        throw new NotImplementedException();
    }
}

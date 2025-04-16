namespace Civitas.Api.Infrastructure.Repositories;

using Core.Entities;
using Core.Interfaces;
using Data;

/// <summary>
/// The repository for managing employee data.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    /// <summary>
    /// The database context used for data access.
    /// </summary>
    private readonly IDbContext _context;

    /// <summary>
    /// The constructor for the EmployeeRepository class.
    /// </summary>
    /// <param name="context"></param>
    public EmployeeRepository(IDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await EmployeesAsync();
    }

    public async Task<Employee> GetEmployeeAsync(long identity)
    {
       return await EmployeeAsync(identity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetNumberOfEmployeesAsync()
    {
        var employees = await EmployeesAsync();
        return employees.Count();
    }

    /// <summary>
    /// Retrieves all employees asynchronously from the repository.
    /// </summary>
    /// <returns></returns>
    private async Task<IEnumerable<Employee>> EmployeesAsync()
    {
        // Simulate async work (e.g., like querying a DB)
        await Task.Delay(100); // optional, just to mimic a real async call

        var employees = new List<Employee>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Jansen"},
            new() { Id = 2, FirstName = "Bob", LastName = "Smith" }
        };

        return employees;
    }

    private async Task<Employee> EmployeeAsync(long identity)
    {
        // Simulate async work (e.g., like querying a DB)
        await Task.Delay(100); // optional, just to mimic a real async call

        var employee = new Employee
        {
            Id = identity,
            FirstName = "Mr.",
            LastName = "Jardex"
        };

        return employee;
    }
}

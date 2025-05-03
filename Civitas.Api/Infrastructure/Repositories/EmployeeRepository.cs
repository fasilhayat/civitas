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
    /// <param name="context">The context object.</param>
    public EmployeeRepository(IDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await EmployeesAsync();
    }

    /// <summary>
    /// Get an <see cref="Employee"/>.
    /// </summary>
    /// <param name="identity">The id of the employee to be retrieved</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<Employee?> GetEmployeeAsync(long identity)
    {
       return await EmployeeAsync(identity);
    }

    /// <summary>
    /// Retrievies the number of employees.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task<int> GetNumberOfEmployeesAsync()
    {
        var employees = await EmployeesAsync();
        return employees.Count();
    }

    /// <summary>
    /// Retrieves all employees asynchronously from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task<IEnumerable<Employee>> EmployeesAsync()
    {
        var employees = new List<Employee>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Jansen"},
            new() { Id = 2, FirstName = "Bob", LastName = "Smith" }
        };

        return employees;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    public async Task AddEmployeeAsync(Employee employee)
    {
        // Save the employees to the database for debug
        var key = new DataKey($"datakey-{employee.Id}");
        await _context.SaveHashData(key, employee);
    }

    /// <summary>
    /// For debugging purposes only. Retrieves a specific employee based on the identity.
    /// </summary>
    /// <param name="identity">The unique identifier of the employee.</param>
    /// <returns>The employee object, or null if not found.</returns>
    private async Task<Employee?> EmployeeAsync(long identity)
    {
        var key = new DataKey($"datakey-{identity}");
        var employee = await _context.GetHashData<Employee>(key);

        // Explicitly handle the null case to satisfy the nullable reference type warning.
        return employee ?? null;
    }
}
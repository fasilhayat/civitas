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

    /// <summary>
    /// Get an <see cref="Employee"/>.
    /// </summary>
    /// <param name="identity">The id of the employee to be retrieved</param>
    /// <returns></returns>
    public async Task<Employee> GetEmployeeAsync(long identity)
    {
       return await EmployeeAsync(identity);
    }

    /// <summary>
    /// Retrievies the number of employees.
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
        // Save the employees to the database for debug
        var key = new DataKey("datakey-52");
        var emp = new Employee
        {
            Id = 1,
            FirstName = "Raistlin",
            MiddleName = "L.",
            LastName = "Majere"
        };
        await _context.SaveHashData(key, emp);

        // Simulate async work (e.g., like querying a DB)
        await Task.Delay(100); // optional, just to mimic a real async call

        var employees = new List<Employee>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Jansen"},
            new() { Id = 2, FirstName = "Bob", LastName = "Smith" }
        };

        // Retrieving a specific employee using DataKey DEBUG code
        var datakey = new DataKey("datakey-551");
        var employee = await _context.GetData<Employee>(datakey);
        if (employee != null)
        {
            employees.Add(employee);
        }

        var retrievedEmployee = await _context.GetHashData<Employee>(key);
        if(retrievedEmployee != null)
        {
            employees.Add(retrievedEmployee);
        }
        // Retrieving a specific employee using DataKey DEBUG code

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

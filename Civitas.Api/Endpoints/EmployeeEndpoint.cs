using System.Security.Cryptography;
using Civitas.Api.Core.Entities;

namespace Civitas.Api.Endpoints;

using Application.Services;

/// <summary>
/// Endpoints for the Employee API. Handles all requests related to Employee.
/// </summary>
public static class EmployeeEndpoint
{
    /// <summary>
    /// Maps all the endpoints for the Employee API.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder to configure the routes.</param>
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var employee = endpoints.MapGroup("/v1/employee").WithTags("Employee");

        employee.MapGet("/list",
            static (EmployeeService employeeSevice) => GetEmployees(employeeSevice));

        employee.MapGet("/id/{identity}",
        static (long identity, EmployeeService employeeSevice) => GetEmployee(identity, employeeSevice));

        employee.MapGet("/count",
            static (EmployeeService employeeSevice) => GetNumberOfEmployees(employeeSevice));

        employee.MapPost("/add",
            static (EmployeeService employeeSevice, string firstName, string? middleName, string lastName) => AddEmployee(employeeSevice, firstName, middleName, lastName));
    }

    /// <summary>
    /// Gets list of employees.
    /// </summary>
    /// <param name="employeeService">The service to handle Employee operations.</param>
    /// <returns>An <see cref="IResult"/> containing the count or an error message if not found.</returns>
    private static async Task<IResult> GetEmployees(EmployeeService employeeService)
    {
        var employees = await employeeService.GetEmployeesAsync();

        var enumerable = employees.ToList();
        return !enumerable.Any() ? Results.Json(new { message = "Employees not found" }, statusCode: 404) : Results.Json(enumerable, statusCode: 200);
    }

    /// <summary>
    /// Gets the employee associated with a specific Employee by ID.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <param name="employeeService">The service to handle Employee operations.</param>
    /// <returns>Returns the employee information.</returns>
    private static async Task<IResult> GetEmployee(long identity, EmployeeService employeeService)
    {
        var employee = await employeeService.GetEmployeeAsync(identity);
        return employee == null ? Results.Json(new { message = "Employee not found" }, statusCode: 404) : Results.Json(employee, statusCode: 200);
    }

    /// <summary>
    /// Gets the number of employees associated with a specific Employee by ID.
    /// </summary>
    /// <param name="employeeService">The service to handle Employee operations.</param>
    /// <returns>An <see cref="IResult"/> containing the count or an error message if not found.</returns>
    private static async Task<IResult> GetNumberOfEmployees(EmployeeService employeeService)
    {
        var antal = await employeeService.GetNumberOfEmployeesAsync();
        return antal == -1
            ? Results.Content("Employeer not found", contentType: "application/json", statusCode: 404)
            : Results.Content(antal.ToString(), contentType: "application/json", statusCode: 200);
    }

    /// <summary>
    /// Adds a new employee.
    /// </summary>
    /// <param name="employeeService">The service to handle Employee operations.</param>
    /// <param name="firstName">The first name of the employee</param>
    /// <param name="middleName">The middle name of the employee</param>   
    /// <param name="lastName">The last name of the employee</param>
    /// <returns>An <see cref="IResult"/>Containing the error message if not added.</returns>
    private static async Task<IResult> AddEmployee(EmployeeService employeeService, string firstName, string? middleName, string lastName)
    {
        var employee = new Employee
        {
            Id = GetHashBasedChecksum(Guid.NewGuid()),
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName
        };
        await employeeService.AddEmployeeAsync(employee);
        return Results.Json(
            new { success = true, employeeId = employee.Id },
            statusCode: StatusCodes.Status201Created
        );

    }

    /// <summary>
    /// For debugging purposes only. Retrieves a specific employee based on the identity. TODO: Delete this method. Not for production.
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    private static int GetHashBasedChecksum(Guid guid)
    {
        using var md5 = MD5.Create();
        byte[] hash = md5.ComputeHash(guid.ToByteArray());
        return BitConverter.ToInt32(hash, 0);
    }
}
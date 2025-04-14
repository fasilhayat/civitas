namespace Civitas.Api.Endpoints;

using Application.Services;

/// <summary>
/// Endpoints for the Salary API. Handles all requests related to employees salaries.
/// </summary>
public static class SalaryEndpoint
{
    /// <summary>
    /// Maps all the endpoints for the Salary API.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder to configure the routes.</param>
    public static void MapSalaryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var employee = endpoints.MapGroup("/v1/salary").WithTags("Salary");

        employee.MapGet("/identity/{id}",
            static (string id, EmployeeService employeeSevice) => Salary(id, employeeSevice));
    }

    /// <summary>
    /// Gets the number of employees associated with a specific Employee by ID.
    /// </summary>
    /// <param name="id">Id of the employee</param>
    /// <param name="employeeService">The service to handle Employee operations.</param>
    /// <returns>An <see cref="IResult"/> containing the count or an error message if not found.</returns>
    private static async Task<IResult> Salary(string id, EmployeeService employeeService)
    {
        var antal = await employeeService.NumberOfEmployeesAsync();
        return antal == -1
            ? Results.Content("Employeer not found", contentType: "application/json", statusCode: 404)
            : Results.Content(antal.ToString(), contentType: "application/json", statusCode: 200);
    }
}

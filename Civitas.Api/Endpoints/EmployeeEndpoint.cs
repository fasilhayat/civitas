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
        var employee = endpoints.MapGroup("/v1/employee").WithTags("employee");

        employee.MapGet("/count",
            static (EmployeeService employeeSevice) => NumberOfEmployees(employeeSevice));

    }

    /// <summary>
    /// Gets the number of employees associated with a specific Employee by ID.
    /// </summary>
    /// <param name="employeeService">The service to handle Employee operations.</param>
    /// <returns>An <see cref="IResult"/> containing the count or an error message if not found.</returns>
    private static async Task<IResult> NumberOfEmployees(EmployeeService employeeService)
    {
        var antal = await employeeService.NumberOfEmployeesAsync();
        return antal == -1
            ? Results.Content("Employeer not found", contentType: "application/json", statusCode: 404)
            : Results.Content(antal.ToString(), contentType: "application/json", statusCode: 200);
    }
}

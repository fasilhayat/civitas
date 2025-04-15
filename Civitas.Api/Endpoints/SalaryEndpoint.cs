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

        employee.MapGet("/identity/{identity}",
            static (long identity, SalaryService salaryService) => Salary(identity, salaryService));
    }

    /// <summary>
    /// Gets the salary information of an employee associated with a specific ID.
    /// </summary>
    /// <param name="identity">Id of the employee</param>
    /// <param name="salaryService">The service to handle Employee operations.</param>
    /// <returns>An <see cref="IResult"/> containing the count or an error message if not found.</returns>
    private static async Task<IResult> Salary(long identity, SalaryService salaryService)
    {
        var salary = await salaryService.GetEmployeeSalaryAsync(identity);
        return salary == null ? Results.Json(new { message = "Employee not found" }, statusCode: 404) : Results.Json(salary, statusCode: 200);
    }
}

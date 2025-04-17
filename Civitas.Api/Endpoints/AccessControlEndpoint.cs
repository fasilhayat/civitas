using Civitas.Api.Application.Services;

namespace Civitas.Api.Endpoints;

/// <summary>
/// Access Control Endpoint.
/// </summary>
public static class AccessControlEndpoint
{
    /// <summary>
    /// Access Control Endpoint.
    /// </summary>
    public static void MapAccessControlEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var employee = endpoints.MapGroup("/v1/access").WithTags("AccessControl");

        employee.MapGet("/id/{identity}",
            static (long identity, AccessControlService accessControlSevice) => GetEmployeeAccessControl(identity, accessControlSevice));

    }

    /// <summary>
    /// Gets the access control information of an employee associated with a specific ID.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <param name="accessControlSevice">The service to handle Employee operations.</param>
    /// <returns>The access control information of the employee.</returns>
    private static async Task<IResult> GetEmployeeAccessControl(long identity, AccessControlService accessControlSevice)
    {
        var accessControl = await accessControlSevice.GetEmployeeAccessControl(identity);
        return accessControl == null ? Results.Json(new { message = "Access control not found" }, statusCode: 404) : Results.Json(accessControl, statusCode: 200);
    }
}
    
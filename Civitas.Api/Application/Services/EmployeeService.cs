namespace Civitas.Api.Application.Services;

using Akka.Actor;
using Core.Entities;
using Core.Interfaces;

/// <summary>
/// The EmployeeService class provides methods to manage employee data.
/// </summary>
public class EmployeeService
{
    /// <summary>
    /// The repository used to access employee data.
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;


    /// <summary>
    /// The reliable delivery actor used for message delivery.
    /// </summary>
    private readonly IActorRef _reliableDeliveryActor;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="employeeRepository">The employee repository.</param>
    /// <param name="reliableDeliveryActor"></param>
    public EmployeeService(IEmployeeRepository employeeRepository, IActorRef reliableDeliveryActor)
    {
        _employeeRepository = employeeRepository;
        _reliableDeliveryActor = reliableDeliveryActor ?? throw new ArgumentNullException(nameof(reliableDeliveryActor));
    }

    /// <summary>
    /// Get number of employees asynchronously.
    /// </summary>
    /// <returns>Returns the full list of employees</returns>
    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _employeeRepository.GetEmployeesAsync();
    }

    /// <summary>
    /// Get employee asynchronously.
    /// </summary>
    /// <param name="identity">The identity of the employee</param>
    /// <returns>Returnsns the employee based on the identity</returns>
    public async Task<Employee?> GetEmployeeAsync(long identity)
    {
        return await _employeeRepository.GetEmployeeAsync(identity);
    }

    /// <summary>
    /// Get number of employees asynchronously.
    /// </summary>
    /// <returns>Returns the number of employees</returns>
    public async Task<int?> GetNumberOfEmployeesAsync()
    {
        return await _employeeRepository.GetNumberOfEmployeesAsync();
    }

    /// <summary>
    /// Adds a new employee asynchronously.
    /// </summary>
    /// <returns></returns>
    public async Task AddEmployeeAsync(Employee employee)
    {
        var callId = "14e339c2-0c63-49f3-abba-d7f01aeacee4";
        var methodKey = "EmployeeRepository.AddEmployee";

        var call = new ReliableMethodCall(
            CallId: callId,
            MethodKey: methodKey,
            Payload: employee
        );

        _reliableDeliveryActor.Tell(call);

        // No direct call to the repository — ReliableDeliveryActor handles it.
        await Task.CompletedTask;
    }
}
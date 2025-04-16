namespace Civitas.Api.Infrastructure.Repositories;

using Core.Entities;
using Core.Interfaces;
using Data;

/// <summary>
/// Repository for managing salary information.
/// </summary>
public class SalaryRepository : ISalaryRepository
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IDbContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public SalaryRepository(IDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Returns a hardcoded salary for a given employee identity.
    /// </summary>
    /// <param name="identity">The employee ID.</param>
    /// <returns>A Task containing a Salary object.</returns>
    public Task<Salary> GetEmployeeSalaryAsync(long identity)
    {
        var salary = new Salary
        {
            Employee = new()
            {
                FirstName = "Malik",
                LastName = "Khan",
                Id = identity,
            },
            EmploymentId = "EMP-" + identity,
            AvailabilityDate = null,
            EmploymentDate = new(2023, 6, 1),
            ProbationPeriodId = 0,
            WorkLocation = "9500",
            SeniorityDate = new(2023, 6, 1),
            JobTitleId = 18887,
            JobTypeId = 18778,
            SalaryAmount = "10.000",
            CostCenter = "Forca",
            StaffCategory = "",
            WorkPercentage = 100,
            Fte = 1.0f,
            WorkingHour = new()
            {
                Start = 8,
                End = 16
            }
        };

        return Task.FromResult(salary);
    }

}
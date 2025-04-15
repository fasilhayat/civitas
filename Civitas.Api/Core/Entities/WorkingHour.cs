namespace Civitas.Api.Core.Entities;

/// <summary>
/// Represents a working hour entity with details about the working hours.
/// </summary>
public class WorkingHour
{
    /// <summary>
    /// Gets or sets the start time of the working hours.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    /// Gets or sets the end time of the working hours.
    /// </summary>
    public int End { get; set; }
}
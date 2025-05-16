namespace Civitas.Api.Infrastructure.Actors;

/// <summary>
/// PendingDeliveries is a message class that represents a list of pending deliveries.
/// </summary>
public sealed class PendingDeliveries
{
    /// <summary>
    /// List of pending deliveries.
    /// </summary>
    public List<string?> Items { get; }

    /// <summary>
    /// Parameterless constructor used to request the list of pending deliveries.
    /// </summary>
    public PendingDeliveries()
    {
        Items = new();
    }

    /// <summary>
    /// Constructor used by the actor to respond with a list of pending deliveries.
    /// </summary>
    /// <param name="items">The pending delivery items.</param>
    public PendingDeliveries(List<string?> items)
    {
        Items = items;
    }
}

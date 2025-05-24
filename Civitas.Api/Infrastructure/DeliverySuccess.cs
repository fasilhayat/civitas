namespace Civitas.Api.Infrastructure;

/// <summary>
/// This record represents a successful delivery of a message.
/// </summary>
/// <param name="DeliveryId">The unique identifier for the delivery.</param>
public sealed record DeliverySuccess(long DeliveryId);

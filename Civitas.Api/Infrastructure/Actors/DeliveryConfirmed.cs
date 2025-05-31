namespace Civitas.Api.Infrastructure.Actors;

/// <summary>
/// This record represents a message confirming the delivery of a method call.
/// </summary>
/// <param name="DeliveryId"></param>
public record DeliveryConfirmed(long DeliveryId);

/// <summary>
/// This record represents a message to check if a delivery has been received.
/// </summary>
/// <param name="DeliveryId"></param>
public record ReceiveDeliveryCheck(long DeliveryId);
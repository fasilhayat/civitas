namespace Civitas.Api.Infrastructure.Actors;

public record DeliveryConfirmed(long DeliveryId);

public record ReceiveDeliveryCheck(long DeliveryId);
namespace Civitas.Api.Core.Entities;

[Serializable]
public record ReliableMethodCall(
    string CallId,
    string MethodKey,
    object Payload,
    long DeliveryId = 0L // will be injected by Deliver
);
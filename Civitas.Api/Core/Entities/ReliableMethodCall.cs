namespace Civitas.Api.Core.Entities;

/// <summary>
/// Represents a reliable method call used for guaranteed delivery via an actor-based system.
/// </summary>
/// <param name="CallId">A unique identifier for this call instance, used for deduplication and traceability.</param>
/// <param name="MethodKey">A string key representing the target method or operation to be invoked.</param>
/// <param name="Payload">The actual data or message to be delivered, typically a serialized object.</param>
/// <param name="DeliveryId">A unique delivery identifier used internally to manage retries and acknowledgments. Injected by the delivery mechanism.</param>
[Serializable]
public record ReliableMethodCall(
    string CallId,
    string MethodKey,
    object Payload,
    long DeliveryId = 0L // will be injected by Deliver
);
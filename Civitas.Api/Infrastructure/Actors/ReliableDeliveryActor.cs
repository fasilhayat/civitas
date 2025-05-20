namespace Civitas.Api.Infrastructure.Actors;

using Akka.Actor;
using Akka.Event;
using Akka.Pattern;
using Akka.Persistence;
using Core.Entities;
using Core.Interfaces;

/// <summary>
/// ReliableDeliveryActor is an actor that handles reliable delivery of messages.
/// </summary>
public class ReliableDeliveryActor : AtLeastOnceDeliveryReceiveActor
{
    /// <summary>
    /// Unique identifier for the actor.
    /// </summary>
    public override string PersistenceId => "reliable-delivery-actor";

    /// <summary>
    /// Logger instance for logging messages.
    /// </summary>
    private readonly ILoggingAdapter _log = Context.GetLogger();

    /// <summary>
    /// Circuit breaker for handling failures in message delivery.
    /// </summary>
    private readonly CircuitBreaker _breaker;

    /// <summary>
    /// Method registry for managing method handlers.
    /// </summary>
    private readonly IMethodRegistry _registry;

    /// <summary>
    /// ReliableDeliveryActor constructor.
    /// </summary>
    /// <param name="breaker">Circuit breaker for handling failures.</param>
    /// <param name="registry">Registry for managing method handlers.</param>
    public ReliableDeliveryActor(CircuitBreaker breaker, IMethodRegistry registry)
    {
        _breaker = breaker;
        _registry = registry;

        RegisterCommandHandlers();
    }

    /// <summary>
    /// Registers command handlers for the actor.
    /// </summary>
    private void RegisterCommandHandlers()
    {
        Command<ReliableMethodCall>(HandleMethodCall);
        Command<DeliveryConfirmed>(HandleDeliveryConfirmed);
        Command<PendingDeliveries>(_ => HandlePendingDeliveries());
    }

    /// <summary>
    /// Handles method call messages.
    /// </summary>
    /// <param name="call">The method call message.</param>
    private void HandleMethodCall(ReliableMethodCall call)
    {
        _log.Info($"Received method invocation for: {call.MethodKey}");
        Persist(call, persisted =>
        {
            TrySendHttp(persisted)
                .ContinueWith(task =>
                {
                    if (task is { IsCompletedSuccessfully: true, Result: true })
                        return (object)new DeliveryConfirmed(call.DeliveryId);

                    return new Status.Failure(task.Exception ?? new Exception("HTTP request failed or returned false"));
                })
                .PipeTo(Self);
        });
    }

    /// <summary>
    /// Tries to send an HTTP request for the method call.
    /// </summary>
    /// <param name="call">Call to be sent.</param>
    /// <returns>Returns a task indicating success or failure.</returns>
    private Task<bool> TrySendHttp(ReliableMethodCall call)
    {
        return _breaker.WithCircuitBreaker(() => TryDeliverMethod(call));
    }

    /// <summary>
    /// Delivers the method call to the appropriate handler.
    /// </summary>
    /// <param name="call">The method call to be delivered.</param>
    /// <returns>Returns a task indicating success or failure.</returns>
    private Task<bool> TryDeliverMethod(ReliableMethodCall call)
    {
        _log.Info($"Delivering method: {call.MethodKey}, deliveryId: {call.DeliveryId}");

        Func<object?, Task<bool>>? handler = null;
        handler = _registry.GetHandler(call.MethodKey);

        var tcs = new TaskCompletionSource<bool>();

        Persist(call, persistedCall =>
        {
            _breaker.WithCircuitBreaker(() => handler(persistedCall.Payload))
                .ContinueWith(task =>
                {
                    if (task is { IsCompletedSuccessfully: true, Result: true })
                    {
                        _log.Info($"Handler succeeded for method {persistedCall.MethodKey}, sending DeliveryConfirmed for deliveryId {persistedCall.DeliveryId}");
                        tcs.SetResult(true);
                        return (object)new DeliveryConfirmed(persistedCall.DeliveryId);
                    }

                    _log.Warning($"Handler failed or returned false for method {persistedCall.MethodKey}: {task.Exception?.Message ?? "Returned false"}");
                    tcs.SetResult(false);
                    return new Status.Failure(task.Exception ?? new Exception($"Handler for {persistedCall.MethodKey} failed or returned false."));
                })
                .PipeTo(Self);
        });

        return tcs.Task;
    }

    /// <summary>
    /// Handles delivery confirmation messages.
    /// </summary>
    /// <param name="confirm">Confirmation message.</param>
    private void HandleDeliveryConfirmed(DeliveryConfirmed confirm)
    {
        _log.Info($"Delivery confirmed: {confirm.DeliveryId}");

        Persist(confirm, _ =>
        {
            ConfirmDelivery(confirm.DeliveryId);
            _log.Info($"Confirmed delivery with id: {confirm.DeliveryId}");
        });
    }

    /// <summary>
    /// Handles delivery confirmation messages.
    /// </summary>
    private void HandlePendingDeliveries()
    {
        var snapshot = GetDeliverySnapshot();
        var pending = snapshot.UnconfirmedDeliveries
            .Select(x => x.Message.ToString())
            .ToList();

        Sender.Tell(new PendingDeliveries(pending));
    }
}
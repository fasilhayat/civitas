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
    public override string PersistenceId => "reliable-delivery-actor";

    private readonly ILoggingAdapter _log = Context.GetLogger();
    private readonly CircuitBreaker _breaker;
    private readonly IMethodRegistry _registry;

    public ReliableDeliveryActor(CircuitBreaker breaker, IMethodRegistry registry)
    {
        _breaker = breaker;
        _registry = registry;

        RegisterCommandHandlers();
        RegisterRecoveryHandlers();
    }

    private void RegisterCommandHandlers()
    {
        Command<ReliableMethodCall>(HandleMethodCall);
        Command<DeliveryConfirmed>(HandleDeliveryConfirmed);
        Command<PendingDeliveries>(HandlePendingDeliveries);
        Command<Status.Failure>(HandleDeliveryFailure);

        // **New: Add explicit handler for ReceiveDeliveryCheck messages**
        Command<ReceiveDeliveryCheck>(HandleReceiveDeliveryCheck);
    }

    private void RegisterRecoveryHandlers()
    {
        Recover<ReliableMethodCall>(call =>
        {
            _log.Info("Recovering ReliableMethodCall: {0}", call.MethodKey);
            Deliver(Self.Path, deliveryId =>
                call with { DeliveryId = deliveryId });
        });

        Recover<DeliveryConfirmed>(confirm =>
        {
            _log.Info("Recovering DeliveryConfirmed: {0}", confirm.DeliveryId);
            ConfirmDelivery(confirm.DeliveryId);
        });
    }

    private void HandleMethodCall(ReliableMethodCall call)
    {
        _log.Info("Received method invocation for: {0}", call.MethodKey);

        var originalSender = Sender;

        Persist(call, persisted =>
        {
            Deliver(Self.Path, deliveryId =>
            {
                var deliverable = persisted with { DeliveryId = deliveryId };
                TryDeliverMethod(deliverable, originalSender);
                return deliverable;
            });
        });
    }

    private void TryDeliverMethod(ReliableMethodCall call, IActorRef originalSender)
    {
        _log.Info("Delivering method: {0}, deliveryId: {1}", call.MethodKey, call.DeliveryId);

        Func<object?, Task<bool>>? handler = null;

        try
        {
            handler = _registry.GetHandler(call.MethodKey);
        }
        catch (Exception ex)
        {
            _log.Warning("Handler not found for {0}: {1}", call.MethodKey, ex.Message);
            return;
        }

        var deliveryResult = _breaker.WithCircuitBreaker(() => handler(call.Payload))
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully && task.Result)
                {
                    _log.Info("Handler succeeded for method {0}, sending DeliveryConfirmed for deliveryId {1}", call.MethodKey, call.DeliveryId);

                    originalSender.Tell(new DeliverySuccess(call.DeliveryId));

                    // Notify self about confirmation to mark delivery as confirmed
                    return (object)new DeliveryConfirmed(call.DeliveryId);
                }

                _log.Warning("Handler failed or returned false for method {0}: {1}", call.MethodKey, task.Exception?.Message ?? "Returned false");
                return new Status.Failure(task.Exception ?? new Exception($"Handler for {call.MethodKey} failed or returned false."));
            });

        deliveryResult.PipeTo(Self);
    }

    private void HandleDeliveryConfirmed(DeliveryConfirmed confirm)
    {
        _log.Info("Delivery confirmed: {0}", confirm.DeliveryId);

        Persist(confirm, _ =>
        {
            ConfirmDelivery(confirm.DeliveryId);
            _log.Info("Confirmed delivery with id: {0}", confirm.DeliveryId);
        });
    }

    private void HandleDeliveryFailure(Status.Failure failure)
    {
        _log.Warning("Delivery failed: {0}", failure.Cause?.Message ?? "Unknown failure");
        // Add retry or alert logic here as needed
    }

    private void HandlePendingDeliveries(PendingDeliveries msg)
    {
        var snapshot = GetDeliverySnapshot();
        var pending = snapshot.UnconfirmedDeliveries
            .Select(x => x.Message?.ToString())
            .ToList();

        Sender.Tell(new PendingDeliveries(pending));
    }

    /// <summary>
    /// Explicit handler for receive delivery check.
    /// This is a message sent by the recipient actor confirming delivery.
    /// </summary>
    /// <param name="check"></param>
    private void HandleReceiveDeliveryCheck(ReceiveDeliveryCheck check)
    {
        _log.Info("Received delivery check for deliveryId: {0}", check.DeliveryId);

        // Verify deliveryId is known and unconfirmed before confirming
        var snapshot = GetDeliverySnapshot();
        if (snapshot.UnconfirmedDeliveries.Any(d => d.DeliveryId == check.DeliveryId))
        {
            _log.Info("Confirming delivery for deliveryId: {0}", check.DeliveryId);
            Persist(new DeliveryConfirmed(check.DeliveryId), _ =>
            {
                ConfirmDelivery(check.DeliveryId);
                _log.Info("Confirmed delivery with id: {0} after receive delivery check", check.DeliveryId);
            });
        }
        else
        {
            _log.Warning("Received delivery check for unknown or already confirmed deliveryId: {0}", check.DeliveryId);
        }
    }
}
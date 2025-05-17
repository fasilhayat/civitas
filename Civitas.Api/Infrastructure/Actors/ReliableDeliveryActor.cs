namespace Civitas.Api.Infrastructure.Actors;

using Akka.Actor;
using Akka.Event;
using Akka.Pattern;
using Akka.Persistence;
using Core.Entities;
using Core.Interfaces;
using System;
using System.Linq;

/// <summary>
/// Actor that handles reliable delivery of method calls.
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
        Command<PendingDeliveries>(_ => HandlePendingDeliveries());
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

        Persist(call, persisted =>
        {
            Deliver(Self.Path, deliveryId =>
                persisted with { DeliveryId = deliveryId });

            TryDeliverMethod(persisted);
        });
    }

    private void TryDeliverMethod(ReliableMethodCall call)
    {
        _log.Info("Delivering method: {0}, deliveryId: {1}", call.MethodKey, call.DeliveryId);

        var handler = _registry.GetHandler(call.MethodKey);
        if (handler == null)
        {
            _log.Warning("No handler registered for method key: {0}", call.MethodKey);
            return;
        }

        _breaker.WithCircuitBreaker(() => handler(call.Payload))
            .ContinueWith(task =>
            {
                if (task is { IsCompletedSuccessfully: true, Result: true })
                    return (object)new DeliveryConfirmed(call.DeliveryId);

                return new Status.Failure(task.Exception ?? new Exception("Method failed or returned false"));
            })
            .PipeTo(Self);
    }

    private void HandleDeliveryConfirmed(DeliveryConfirmed confirm)
    {
        _log.Info("Delivery confirmed: {0}", confirm.DeliveryId);

        Persist(confirm, _ =>
        {
            ConfirmDelivery(confirm.DeliveryId);
        });
    }

    private void HandlePendingDeliveries()
    {
        var snapshot = GetDeliverySnapshot();
        var pending = snapshot.UnconfirmedDeliveries
            .Select(x => x.Message.ToString())
            .ToList();

        Sender.Tell(new PendingDeliveries(pending));
    }
}

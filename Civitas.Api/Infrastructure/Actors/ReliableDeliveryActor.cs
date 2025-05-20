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
    }

    private void RegisterCommandHandlers()
    {
        Command<ReliableMethodCall>(HandleMethodCall);
        Command<DeliveryConfirmed>(HandleDeliveryConfirmed);
        Command<PendingDeliveries>(_ => HandlePendingDeliveries());
    }


    private void HandleMethodCall(ReliableMethodCall call)
    {
        _log.Info("Received method invocation for: {0}", call.MethodKey);
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

            //Deliver(Self.Path, deliveryId =>
            //{
            //    var deliverable = persisted with { DeliveryId = deliveryId };
            //    TryDeliverMethod(deliverable);
            //    return deliverable;
            //});
        });
    }

    private Task<bool> TrySendHttp(ReliableMethodCall call)
    {
        return _breaker.WithCircuitBreaker(() => TryDeliverMethod(call));
    }

    private Task<bool> TryDeliverMethod(ReliableMethodCall call)
    {
        _log.Info("Delivering method: {0}, deliveryId: {1}", call.MethodKey, call.DeliveryId);

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
                        _log.Info("Handler succeeded for method {0}, sending DeliveryConfirmed for deliveryId {1}", persistedCall.MethodKey, persistedCall.DeliveryId);
                        tcs.SetResult(true);
                        return (object)new DeliveryConfirmed(persistedCall.DeliveryId);
                    }

                    _log.Warning("Handler failed or returned false for method {0}: {1}", persistedCall.MethodKey, task.Exception?.Message ?? "Returned false");
                    tcs.SetResult(false);
                    return new Status.Failure(task.Exception ?? new Exception($"Handler for {persistedCall.MethodKey} failed or returned false."));
                })
                .PipeTo(Self);
        });

        return tcs.Task;
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

    private void HandlePendingDeliveries()
    {
        var snapshot = GetDeliverySnapshot();
        var pending = snapshot.UnconfirmedDeliveries
            .Select(x => x.Message.ToString())
            .ToList();

        Sender.Tell(new PendingDeliveries(pending));
    }
}
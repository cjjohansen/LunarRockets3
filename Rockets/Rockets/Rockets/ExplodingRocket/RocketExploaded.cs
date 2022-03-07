using Core.Events;
using Core.Exceptions;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.ExploadingRocket;

public record RocketExploaded(
    Guid RocketId,
    String Reason,
    DateTime ExploadedAt
): IEvent
{
    public static RocketExploaded Create(
        Guid rocketId,
        String reason,
        DateTime exploadedAt)
    {
        return new RocketExploaded(rocketId, reason, exploadedAt);
    }
}

//internal class HandleRocketExploaded: IEventHandler<RocketExploaded>
//{
//    private readonly IQuerySession querySession;
//    private readonly IEventBus eventBus;

//    public HandleRocketExploaded(
//        IQuerySession querySession,
//        IEventBus eventBus
//    )
//    {
//        this.querySession = querySession;
//        this.eventBus = eventBus;
//    }

//    public async Task Handle(ShoppingCartConfirmed @event, CancellationToken cancellationToken)
//    {
//        var cart = await querySession.LoadAsync<Rocket>(@event.CartId, cancellationToken)
//                   ?? throw AggregateNotFoundException.For<Rocket>(@event.CartId);

//        var externalEvent = ShoppingCartFinalized.Create(
//            @event.CartId,
//            cart.ClientId,
//            cart.ProductItems.ToList(),
//            cart.TotalPrice,
//            @event.ConfirmedAt
//        );

//        await eventBus.Publish(externalEvent, cancellationToken);
//    }
//}

using Lunar.Rockets.GettingCartById;
using Core.EventStoreDB.Events;
using Core.Exceptions;
using Core.Queries;
using EventStore.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.GettingCartAtVersion;

public record GetRocketAtVersion(
    Guid CartId,
    ulong Version
): IQuery<RocketDetails>
{
    public static GetRocketAtVersion Create(Guid? cartId, ulong? version)
    {
        if (cartId == null || cartId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(cartId));
        if (version == null)
            throw new ArgumentOutOfRangeException(nameof(version));

        return new GetRocketAtVersion(cartId.Value, version.Value);
    }
}

internal class HandleGetCartAtVersion:
    IQueryHandler<GetRocketAtVersion, RocketDetails>
{
    private readonly EventStoreClient eventStore;

    public HandleGetCartAtVersion(EventStoreClient eventStore)
    {
        this.eventStore = eventStore;
    }

    public async Task<RocketDetails> Handle(GetRocketAtVersion request, CancellationToken cancellationToken)
    {
        var cart = await eventStore.AggregateStream<RocketDetails>(
            request.CartId,
            cancellationToken,
            request.Version
        );

        if (cart == null)
            throw AggregateNotFoundException.For<Rocket>(request.CartId);

        return cart;
    }
}

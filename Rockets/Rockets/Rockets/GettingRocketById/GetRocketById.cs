using Core.Exceptions;
using Core.Queries;
using Marten;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.GettingCartById;

public record GetRocketById(
    Guid CartId
    ) : IQuery<RocketDetails>
{
    public static GetRocketById Create(Guid? cartId)
    {
        if (cartId == null || cartId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(cartId));

        return new GetRocketById(cartId.Value);
    }
}

internal class HandleGetCartById :
    IQueryHandler<GetRocketById, RocketDetails>
{
    private readonly IDocumentSession querySession;

    public HandleGetCartById(IDocumentSession querySession)
    {
        this.querySession = querySession;
    }

    public async Task<RocketDetails> Handle(GetRocketById request, CancellationToken cancellationToken)
    {
        var cart = await querySession.LoadAsync<RocketDetails>(request.CartId, cancellationToken);

        return cart ?? throw AggregateNotFoundException.For<Rocket>(request.CartId);
    }
}

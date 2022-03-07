using Core.Queries;
using Marten;
using Marten.Pagination;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.GettingCartHistory;

public record GetRocketHistory(
    Guid CartId,
    int PageNumber,
    int PageSize
): IQuery<IPagedList<RocketHistory>>
{
    public static GetRocketHistory Create(Guid? cartId, int? pageNumber = 1, int? pageSize = 20)
    {
        if (cartId == null || cartId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(cartId));
        if (pageNumber is null or <= 0)
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        if (pageSize is null or <= 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(pageSize));

        return new GetRocketHistory(cartId.Value, pageNumber.Value, pageSize.Value);
    }
}

internal class HandleGetCartHistory:
    IQueryHandler<GetRocketHistory, IPagedList<RocketHistory>>
{
    private readonly IDocumentSession querySession;

    public HandleGetCartHistory(IDocumentSession querySession)
    {
        this.querySession = querySession;
    }

    public Task<IPagedList<RocketHistory>> Handle(GetRocketHistory query, CancellationToken cancellationToken)
    {
        var (cartId, pageNumber, pageSize) = query;

        return querySession.Query<RocketHistory>()
            .Where(h => h.RocketId == cartId)
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }
}

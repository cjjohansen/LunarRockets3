using Core.Queries;
using Marten;
using Marten.Pagination;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.GettingCarts;

public record GetRockets(
    int PageNumber,
    int PageSize
): IQuery<IPagedList<RocketInfo>>
{
    public static GetRockets Create(int? pageNumber = 1, int? pageSize = 20)
    {
        if (pageNumber is null or <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(pageSize));
        if (pageSize is null or <= 0 or > 100)
            throw new System.ArgumentOutOfRangeException(nameof(pageSize));

        return new GetRockets(pageNumber.Value, pageSize.Value);
    }
}

internal class HandleGetCarts:
    IQueryHandler<GetRockets, IPagedList<RocketInfo>>
{
    private readonly IDocumentSession querySession;

    public HandleGetCarts(IDocumentSession querySession)
    {
        this.querySession = querySession;
    }

    public Task<IPagedList<RocketInfo>> Handle(GetRockets request, CancellationToken cancellationToken)
    {
        var (pageNumber, pageSize) = request;

        return querySession.Query<RocketInfo>()
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);
    }
}

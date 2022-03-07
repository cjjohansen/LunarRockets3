using Core.Commands;
using Core.EventStoreDB.OptimisticConcurrency;
using Core.EventStoreDB.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.ExploadingRocket;

public record ExploadRocket(
    Guid RocketId,
    String reason,
    DateTime exploadedAt
): ICommand
{
    public static ExploadRocket Create(Guid? rocketId, String reason, DateTime exploadedAt)
    {
        if (rocketId == null || rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));


        return new ExploadRocket(rocketId.Value, reason, exploadedAt);
    }
}

internal class HandleExploadRocket :
    ICommandHandler<ExploadRocket>
{
    private readonly IEventStoreDBRepository<Rocket> rocketRepository;
    private readonly IEventStoreDBAppendScope scope;

    public HandleExploadRocket (
        IEventStoreDBRepository<Rocket> rocketRepository,
        IEventStoreDBAppendScope scope
    )
    {
        this.rocketRepository = rocketRepository;
        this.scope = scope;
    }

    public async Task<Unit> Handle(ExploadRocket command, CancellationToken cancellationToken)
    {
        var (rocketId, reason, exploadeAt) = command;

        await scope.Do((expectedRevision, eventMetadata) =>
            rocketRepository.GetAndUpdate(
                rocketId,
                rocket => rocket.Expload( reason, exploadeAt),
                expectedRevision,
                eventMetadata,
                cancellationToken
            )
        );

        return Unit.Value;
    }
}

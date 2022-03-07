using Core.Commands;
using Core.EventStoreDB.OptimisticConcurrency;
using Core.EventStoreDB.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.IncreasingSpeed;

public record IncreaseRocketSpeed(
    Guid RocketId,
    long Increment
): ICommand
{
    public static IncreaseRocketSpeed Create(Guid? rocketId, long speedIncrement)
    {
        if (rocketId == null || rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));


        return new IncreaseRocketSpeed(rocketId.Value, speedIncrement);
    }
}

internal class HandleIncreaseRocketSpeed :
    ICommandHandler<IncreaseRocketSpeed>
{
    private readonly IEventStoreDBRepository<Rocket> rocketRepository;
    private readonly IEventStoreDBAppendScope scope;

    public HandleIncreaseRocketSpeed (
        IEventStoreDBRepository<Rocket> rocketRepository,
        IEventStoreDBAppendScope scope
    )
    {
        this.rocketRepository = rocketRepository;
        this.scope = scope;
    }

    public async Task<Unit> Handle(IncreaseRocketSpeed command, CancellationToken cancellationToken)
    {
        var (rocketId, speedIncrement) = command;

        await scope.Do((expectedRevision, eventMetadata) =>
            rocketRepository.GetAndUpdate(
                rocketId,
                rocket => rocket.IncreaseRocketSpeed( speedIncrement),
                expectedRevision,
                eventMetadata,
                cancellationToken
            )
        );

        return Unit.Value;
    }
}

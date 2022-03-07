using Core.Commands;
using Core.EventStoreDB.OptimisticConcurrency;
using Core.EventStoreDB.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.DecreasingSpeed;

public record DecreaseRocketSpeed(
    Guid RocketId,
    long Increment
): ICommand
{
    public static DecreaseRocketSpeed Create(Guid? rocketId, long speedIncrement)
    {
        if (rocketId == null || rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));


        return new DecreaseRocketSpeed(rocketId.Value, speedIncrement);
    }
}

internal class HandleDecreaseRocketSpeed :
    ICommandHandler<DecreaseRocketSpeed>
{
    private readonly IEventStoreDBRepository<Rocket> rocketRepository;
    private readonly IEventStoreDBAppendScope scope;

    public HandleDecreaseRocketSpeed (
        IEventStoreDBRepository<Rocket> rocketRepository,
        IEventStoreDBAppendScope scope
    )
    {
        this.rocketRepository = rocketRepository;
        this.scope = scope;
    }

    public async Task<Unit> Handle(DecreaseRocketSpeed command, CancellationToken cancellationToken)
    {
        var (rocketId, speedDecrement) = command;

        await scope.Do((expectedRevision, eventMetadata) =>
            rocketRepository.GetAndUpdate(
                rocketId,
                rocket => rocket.IncreaseRocketSpeed( speedDecrement),
                expectedRevision,
                eventMetadata,
                cancellationToken
            )
        );

        return Unit.Value;
    }
}

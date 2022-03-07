using Core.Commands;
using Core.EventStoreDB.OptimisticConcurrency;
using Core.EventStoreDB.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.ChangingMission;

public record ChangeMission(
    Guid RocketId,
    string NewMission
): ICommand
{
    public static ChangeMission Create(Guid? rocketId,string newMission)
    {
        if (rocketId == null || rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));
       

        return new ChangeMission(rocketId.Value,  newMission);
    }
}

internal class HandleChangeMission:
    ICommandHandler<ChangeMission>
{
    private readonly IEventStoreDBRepository<Rocket> rocketRepository;
    private readonly IEventStoreDBAppendScope scope;

    public HandleChangeMission(
        IEventStoreDBRepository<Rocket> rocketRepository,
        IEventStoreDBAppendScope scope
    )
    {
        this.rocketRepository = rocketRepository;
        this.scope = scope;
    }

    public async Task<Unit> Handle(ChangeMission command, CancellationToken cancellationToken)
    {
        var (rocketId, newMission) = command;

        await scope.Do((expectedRevision, eventMetadata) =>
            rocketRepository.GetAndUpdate(
                rocketId,
                rocket => rocket.ChangeMission(newMission),
                expectedRevision,
                eventMetadata,
                cancellationToken
            )
        );

        return Unit.Value;
    }
}

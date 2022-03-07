using Core.Commands;
using Core.EventStoreDB.OptimisticConcurrency;
using Core.EventStoreDB.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunar.Rockets.Launching;


public record LaunchRocket(
    Guid RocketId,
    uint MessageNumber,
    DateTime MessageTime,
    String Type,
    String Mission,
    long Speed
) : ICommand
{
    public static LaunchRocket Create(Guid rocketId, uint messageNumber, DateTime messageTime, String type, String mission, long speed)
    {
        if (rocketId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(rocketId));
        if (String.IsNullOrEmpty(type))
            throw new ArgumentOutOfRangeException(nameof(type));
        if (String.IsNullOrEmpty(mission))
            throw new ArgumentOutOfRangeException(nameof(mission));

        //TODO: Check all parameters

        return new LaunchRocket(rocketId, messageNumber, messageTime, type, mission, speed);
    }
}





internal class HandleLaunchRocket:
    ICommandHandler<LaunchRocket>
{
    private readonly IEventStoreDBRepository<Rocket> rocketRepository;
    private readonly IEventStoreDBAppendScope scope;

    public HandleLaunchRocket(
        IEventStoreDBRepository<Rocket> rocketRepository,
        IEventStoreDBAppendScope scope
    )
    {
        this.rocketRepository = rocketRepository;
        this.scope = scope;
    }

    public async Task<Unit> Handle(LaunchRocket command, CancellationToken cancellationToken)
    {
        var (rocketId, messageNumber, messageTime, type, mission, speed ) = command;

        await scope.Do((_, eventMetadata) =>
            rocketRepository.Add(
                Rocket.Launch(rocketId, messageNumber, messageTime, type, mission, speed),
                eventMetadata,
                cancellationToken
            )
        );
        return Unit.Value;
    }
}

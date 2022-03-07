using Lunar.Rockets.Launching;
using Lunar.Rockets.ChangingMission;
using Core.Projections;
using System;
using Lunar.Rockets.IncreasingSpeed;
using Lunar.Rockets.ExploadingRocket;
using Lunar.Rockets.DecreasingSpeed;

namespace Lunar.Rockets.GettingCartHistory;

public class RocketHistory: IVersionedProjection
{
    public Guid Id { get; set;}
    public Guid RocketId { get; set;}
    public string Description { get; set; } = default!;
    public ulong LastProcessedPosition { get; set; }

    public void When(object @event)
    {
        switch (@event)
        {
            case RocketLaunched rocketLaunched:
                Apply(rocketLaunched);
                return;
            case RocketSpeedIncreased rocketSpeedIncreased:
                Apply(rocketSpeedIncreased);
                return;
            case RocketSpeedDecreased rocketSpeedDecreased:
                Apply(rocketSpeedDecreased);
                return;
            case MissionChanged cartInitialized:
                Apply(cartInitialized);
                return;
            case RocketExploaded rocketExploaded:
                Apply(rocketExploaded);
                return;
        }
    }

    public void Apply(RocketLaunched @event)
    {
        Id = Guid.NewGuid();
        RocketId = @event.RocketId;
        Description = $"Rocket of type {@event.Type} Launched at {@event.MessageTime} on mission {@event.Mission} cruising at speed { @event.Speed} with id {@event.RocketId}";
    }

    public void Apply(RocketSpeedIncreased @event)
    {
        Id = Guid.NewGuid();
        RocketId = @event.RocketId;
        Description = $"Rocket increased speed by {@event.SpeedIncrement}";
    }

    public void Apply(RocketSpeedDecreased @event)
    {
        Id = Guid.NewGuid();
        RocketId = @event.RocketId;
        Description = $"Rocket decreased speed by {@event.SpeedDecrement}";
    }


    public void Apply(MissionChanged @event)
    {
        Id = Guid.NewGuid();
        RocketId = @event.RocketId;
        Description = $"Rocket Mission changed from {@event.PreviousMission} to {@event.NewMission }";
    }

    public void Apply(RocketExploaded @event)
    {
        Id = Guid.NewGuid();
        RocketId = @event.RocketId;
        Description = $"Rocket exploaded at {@event.ExploadedAt}";
    }
}

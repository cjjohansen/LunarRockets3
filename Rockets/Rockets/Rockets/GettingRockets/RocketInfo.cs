using Lunar.Rockets.Launching;
using Lunar.Rockets.ChangingMission;
using Core.Projections;
using System;
using Lunar.Rockets.IncreasingSpeed;
using Lunar.Rockets.DecreasingSpeed;
using Lunar.Rockets.ExploadingRocket;

namespace Lunar.Rockets.GettingCarts;

public class RocketInfo: IVersionedProjection
{
    public Guid Id { get; set; }
    public long Speed { get; set; }
    public string Mission { get; set; }
    public string Type { get; set; }

    public RocketStatus Status { get; set; }

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
            case MissionChanged missionChanged:
                Apply(missionChanged);
                return;
            case RocketExploaded rocketExploaded:
                Apply(rocketExploaded);
                return;
        }
    }

    public void Apply(RocketLaunched @event)
    {
        Id = @event.RocketId;
        Type = @event.Type;
        Mission = @event.Mission;
        Speed = @event.Speed;
        Status = @event.Status;
    }

    public void Apply(RocketSpeedIncreased @event)
    {
        Speed += @event.SpeedIncrement;
    }

    public void Apply(RocketSpeedDecreased @event)
    {
        Speed -= @event.SpeedDecrement;
    }

    public void Apply(MissionChanged @event)
    {
        Mission = @event.NewMission;
    }

    public void Apply(RocketExploaded @event)
    {
        Status = RocketStatus.Exploaded;
    }
}

using Lunar.Rockets.ChangingMission;
using Core.Extensions;
using Core.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using Lunar.Rockets.Launching;
using Lunar.Rockets.IncreasingSpeed;
using Lunar.Rockets.DecreasingSpeed;
using Lunar.Rockets.ExploadingRocket;

namespace Lunar.Rockets.GettingCartById;

public class RocketDetails: IVersionedProjection
{
    public Guid Id { get; set; }
    
    public RocketStatus Status { get; set; }

    public string Mission { get; set; }
    public string Type { get; set; }

    public long Speed { get; set; }


    public long Version { get; set; }


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
        Version = 0;
    }

    public void Apply(RocketSpeedIncreased @event)
    {
        Version++;

        Speed = Speed + @event.SpeedIncrement;
    }

    public void Apply(RocketSpeedDecreased @event)
    {
        Version++;

        Speed = Speed - @event.SpeedDecrement;
    }

    public void Apply(MissionChanged @event)
    {
        Version++;

        Mission = @event.NewMission;
    }

    public void Apply(RocketExploaded @event)
    {
        Version++;

        Status = RocketStatus.Exploaded;
    }

    public ulong LastProcessedPosition { get; set; }
}

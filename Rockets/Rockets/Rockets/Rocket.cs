using Lunar.Rockets.Launching;
using Core.Aggregates;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Lunar.Rockets.IncreasingSpeed;
using Lunar.Rockets.ExploadingRocket;
using Lunar.Rockets.ChangingMission;
using Lunar.Rockets.DecreasingSpeed;

namespace Lunar.Rockets;

public class Rocket: Aggregate
{
    public RocketStatus Status { get; private set; }
    public long Speed { get; private set; }

    public string Mission { get; private set; }

    public string Type { get; private set; }


    public static Rocket Launch(
        Guid rocketId, long messageNumber, DateTime messageTime, String type, String mission, long speed)
    {
        return new Rocket(rocketId, messageNumber, messageTime, type, mission, speed);
    }

    private Rocket(){}

    public override void When(object @event)
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
            case RocketExploaded cartInitialized:
                Apply(cartInitialized);
                return;
        }
    }

    private Rocket(
             Guid rocketId, long messageNumber, DateTime messageTime, String type, String mission, long speed)
    {
        var @event = RocketLaunched.Create(
            rocketId,
            messageNumber,
            messageTime,
            type,
            mission,
            speed,
            RocketStatus.Alive
        );

        Enqueue(@event);
        Apply(@event);
    }

    private void Apply(RocketLaunched @event)
    {
        Version++;

        Id = @event.RocketId;

        Status = RocketStatus.Alive;
        Type = @event.Type;
        Mission = @event.Mission;
        Speed = @event.Speed;

    }

    public void IncreaseRocketSpeed(
        long speedIncrement)
    {
      
        var @event = RocketSpeedIncreased.Create(Id, speedIncrement);

        Enqueue(@event);
        Apply(@event);
    }

    private void Apply(RocketSpeedIncreased @event)
    {
        Version++;

        Speed += @event.SpeedIncrement;
    }

    public void DecreaseRocketSpeed(
       long speedDecrement)
    {

        var @event = RocketSpeedDecreased.Create(Id, speedDecrement);

        Enqueue(@event);
        Apply(@event);
    }

    private void Apply(RocketSpeedDecreased @event)
    {
        Version++;

        Speed -= @event.SpeedDecrement;
    }




    public void ChangeMission(
        string newMission)
    {
        if(Status != RocketStatus.Alive)
            throw new InvalidOperationException($"Sorry the Rocket is exploaded");

      
        //TODO: Check other rules (We really just accpet events so actually there should not be anything to validate


        var @event = MissionChanged.Create(Id, Mission, newMission);

        Enqueue(@event);
        Apply(@event);
    }

    private void Apply(MissionChanged @event)
    {
        Version++;
        Mission = @event.NewMission;
        
       
    }

    public void Expload(String reason,DateTime exploadedAt)
    {
        if(Status == RocketStatus.Exploaded)
            throw new InvalidOperationException($"Rocket is allready exploaded.");

        var @event = RocketExploaded.Create(Id, reason, exploadedAt);

        Enqueue(@event);
        Apply(@event);
    }

    private void Apply(RocketExploaded @event)
    {
        Version++;
        Status = RocketStatus.Exploaded;
    }
  
}

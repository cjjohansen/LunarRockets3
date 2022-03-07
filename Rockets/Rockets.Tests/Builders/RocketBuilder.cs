using Lunar.Rockets;
using Lunar.Rockets.Launching;
using Core.Events;
using System;
using System.Collections.Generic;

namespace Rockets.Tests.Builders;

internal class RocketBuilder
{
    private readonly Queue<IEvent> eventsToApply = new();

    public RocketBuilder Initialized()
    {
        var rocketId = Guid.NewGuid();
       

        eventsToApply.Enqueue(new RocketLaunched(rocketId,0, DateTime.UtcNow, "Falcon-9","ARTEMIS", 3000, RocketStatus.Alive));

        return this;
    }

    public static RocketBuilder Create() => new();

    public Rocket Build()
    {
        var rocket = (Rocket) Activator.CreateInstance(typeof(Rocket), true)!;

        foreach (var @event in eventsToApply)
        {
            rocket.When(@event);
        }

        return rocket;
    }
}
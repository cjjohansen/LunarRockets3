using Lunar.Rockets;
using Lunar.Rockets.Launching;
using Core.Testing;
using FluentAssertions;
using System;

namespace Rockets.Tests.Extensions.Reservations;

internal static class CartExtensions
{
    public static Rocket IsLaunchedRocketWith(
        this Rocket rocket,
        Guid id,
        string  type,
        string mission,
        long speed
        )
    {

        rocket.Id.Should().Be(id);
        rocket.Type.Should().Be(type);
        rocket.Status.Should().Be(RocketStatus.Alive);
        rocket.Mission.Should().Be(mission);
        rocket.Speed.Should().Be(speed);
        rocket.Version.Should().Be(1);

        return rocket;
    }

    public static Rocket HasRocketLaunchedEventWith(
        this Rocket rocket,
        Guid id,
        string type,
        string mission,
        long speed
        )
    {
        var @event = rocket.PublishedEvent<RocketLaunched>();

        @event.Should().NotBeNull();
        @event.Should().BeOfType<RocketLaunched>();
        @event!.RocketId.Should().Be(id);

       
        @event.Status.Should().Be(RocketStatus.Alive);

        @event.Type.Should().Be(type);
        @event.Status.Should().Be(RocketStatus.Alive);
        @event.Mission.Should().Be(mission);
        @event.Speed.Should().Be(speed);

        return rocket;
    }
}
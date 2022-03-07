using Lunar.Rockets;
using Rockets.Tests.Extensions.Reservations;
using System;
using Xunit;

namespace Rockets.Tests.Rockets.Launching;

public class LaunchRocketTests
{
    [Fact]
    public void ForValidParams_ShouldCreateRocketWithAliveStatus()
    {
        // Given
        var rocketId = Guid.NewGuid();

        // When
        var rocket = Rocket.Launch(
            rocketId,
            0,
            DateTime.UtcNow,
            "Falcon-9",
            "Artemis",
            500
        );

        // Then

        rocket
            .IsLaunchedRocketWith(
                rocketId,
                "Falcon-9",
                "Artemis",
                500
            )
            .HasRocketLaunchedEventWith(
                rocketId,
                "Falcon-9",
                "Artemis",
                500
            );
    }
}
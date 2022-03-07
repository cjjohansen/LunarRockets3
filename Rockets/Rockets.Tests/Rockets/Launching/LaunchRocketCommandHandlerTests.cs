using Lunar.Rockets;
using Lunar.Rockets.Launching;
using Rockets.Tests.Extensions.Reservations;
using Rockets.Tests.Stubs.Events;
using Rockets.Tests.Stubs.Repositories;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Rockets.Tests.Rockets.Launching;

public class LaunchRocketCommandHandlerTests
{
    [Fact]
    public async Task ForLaunchRocketCommand_ShouldLaunchRocket()
    {
        // Given
        var repository = new FakeRepository<Rocket>();
        var scope = new DummyEventStoreDBAppendScope();

        var commandHandler = new HandleLaunchRocket(
            repository,
            scope
        );

        var command = LaunchRocket.Create(
            Guid.NewGuid(),
            0,
            DateTime.UtcNow,
            "Falcon-9",
            "ARTEMIS",
            4000
            );

        // When
        await commandHandler.Handle(command, CancellationToken.None);

        //Then
        repository.Aggregates.Should().HaveCount(1);

        var rocket = repository.Aggregates.Values.Single();

        rocket
            .IsLaunchedRocketWith(
                command.RocketId,
                command.Type,
                command.Mission,
                command.Speed
            )
            .HasRocketLaunchedEventWith(
                command.RocketId,
                command.Type,
                command.Mission,
                command.Speed
            );
    }
}

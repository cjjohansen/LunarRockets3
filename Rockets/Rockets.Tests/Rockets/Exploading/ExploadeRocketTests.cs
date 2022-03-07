using Lunar.Rockets;
using Rockets.Tests.Builders;
using Core.Testing;
using FluentAssertions;
using Xunit;
using Lunar.Rockets.ExploadingRocket;

namespace Rockets.Tests.Rockets.ConfirmingCart;

public class ExploadeRocketTests
{
    [Fact]
    public void ForAliveRocket_ShouldSucceed()
    {
        // Given
        var rocket = RocketBuilder
            .Create()
            .Initialized()
            .Build();

        // When
        rocket.Expload("Critical misile hit", System.DateTime.UtcNow);

        // Then
        rocket.Status.Should().Be(RocketStatus.Exploaded);
        rocket.Version.Should().Be(2);

        var @event = rocket.PublishedEvent<RocketExploaded>();

        @event.Should().NotBeNull();
        @event.Should().BeOfType<RocketExploaded>();
        @event!.RocketId.Should().Be(rocket.Id);
    }
}
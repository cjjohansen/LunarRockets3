using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Rockets.Api.Requests;
using Core.Api.Testing;
using FluentAssertions;
using Xunit;

namespace Rockets.Api.Tests.Rockets.Launching { 

public class ConsumeRocketEventFixture: ApiFixture<Startup>
{
    protected override string ApiUrl => "/api/Rockets";

    public readonly Guid ChannelId = Guid.NewGuid();

    public HttpResponseMessage CommandResponse = default!;

    public override async Task InitializeAsync()
    {

        CommandResponse = await Post(
            new RocketEvent(
                new MetaData(ChannelId,0,DateTimeOffset.UtcNow,"RocketLaunched"),
                new
                {
                    type= "Falcon-9",
                    launchSpeed = 500,
                    mission = "ARTEMIS"
                }
            )
        );
    }
}

    public class ConsumeRocketEventsTests : IClassFixture<ConsumeRocketEventFixture>
    {
        private readonly ConsumeRocketEventFixture fixture;

        public ConsumeRocketEventsTests(ConsumeRocketEventFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        [Trait("Category", "Acceptance")]
        public async Task Post_Should_Return_AcceptedStatus_With_ChannelId()
        {
            var commandResponse = fixture.CommandResponse.EnsureSuccessStatusCode();
            commandResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);

            // get accpeted record id
            var acceptedId = await commandResponse.GetResultFromJson<Guid>();
            acceptedId.Should().NotBeEmpty();
        }
    }
}

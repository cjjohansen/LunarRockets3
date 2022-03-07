using Lunar.Rockets.GettingCartAtVersion;
using Lunar.Rockets.GettingCartById;
using Lunar.Rockets.GettingCartHistory;
using Lunar.Rockets.GettingCarts;
using Lunar.Rockets.Launching;
using Lunar.Rockets.IncreasingSpeed;
using Lunar.Rockets.DecreasingSpeed;
using Lunar.Rockets.ChangingMission;
using Core.Commands;
using Core.EventStoreDB.Repository;
using Core.Marten.ExternalProjections;
using Core.Queries;
using Marten.Pagination;
using Microsoft.Extensions.DependencyInjection;
using Lunar.Rockets.ExploadingRocket;

namespace Lunar.Rockets;

internal static class RocketsConfig
{
    internal static IServiceCollection AddRockets(this IServiceCollection services)
    {
        return services
            .AddScoped<IEventStoreDBRepository<Rocket>, EventStoreDBRepository<Rocket>>()
            .AddCommandHandlers()
            .AddProjections()
            .AddQueryHandlers();
    }

    private static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        return services
            .AddCommandHandler<LaunchRocket, HandleLaunchRocket>()
            .AddCommandHandler<IncreaseRocketSpeed, HandleIncreaseRocketSpeed>()
            .AddCommandHandler<DecreaseRocketSpeed, HandleDecreaseRocketSpeed>()
            .AddCommandHandler<ChangeMission, HandleChangeMission>()
            .AddCommandHandler<ExploadRocket, HandleExploadRocket>();


    }

    private static IServiceCollection AddProjections(this IServiceCollection services)
    {
        services
            .Project<RocketLaunched, RocketDetails>(@event => @event.RocketId)
            .Project<RocketSpeedIncreased, RocketDetails>(@event => @event.RocketId)
            .Project<RocketSpeedDecreased, RocketDetails>(@event => @event.RocketId)
            .Project<MissionChanged, RocketDetails>(@event => @event.RocketId)
            .Project<RocketExploaded, RocketDetails>(@event => @event.RocketId);

        services
            .Project<RocketLaunched, RocketInfo>(@event => @event.RocketId)
            .Project<RocketSpeedIncreased, RocketInfo>(@event => @event.RocketId)
            .Project<RocketSpeedDecreased, RocketInfo>(@event => @event.RocketId)
            .Project<MissionChanged, RocketInfo>(@event => @event.RocketId)
            .Project<RocketExploaded, RocketInfo>(@event => @event.RocketId);

        services
           .Project<RocketLaunched, RocketHistory>(@event => @event.RocketId)
            .Project<RocketSpeedIncreased, RocketHistory>(@event => @event.RocketId)
            .Project<RocketSpeedDecreased, RocketHistory>(@event => @event.RocketId)
            .Project<MissionChanged, RocketHistory>(@event => @event.RocketId)
            .Project<RocketExploaded, RocketHistory>(@event => @event.RocketId);

        return services;
    }

    private static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        return services
            .AddQueryHandler<GetRocketById, RocketDetails, HandleGetCartById>()
            .AddQueryHandler<GetRockets, IPagedList<RocketInfo>, HandleGetCarts>()
            .AddQueryHandler<GetRocketHistory, IPagedList<RocketHistory>, HandleGetCartHistory>()
            .AddQueryHandler<GetRocketAtVersion, RocketDetails, HandleGetCartAtVersion>();
    }
}

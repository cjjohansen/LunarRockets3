using Core.Commands;
using Core.Events;
using Core.Events.External;
using Core.Ids;
using Core.Queries;
using Core.Requests;
using Core.Tracing;
using Core.Tracing.Causation;
using Core.Tracing.Correlation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Core;

public static class Config
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddMediatR()
            .AddScoped<ICommandBus, CommandBus>()
            .AddScoped<IQueryBus, QueryBus>()
            .AddTracing()
            .AddEventBus();

        services.TryAddScoped<IExternalEventProducer, NulloExternalEventProducer>();
        services.TryAddScoped<IExternalCommandBus, ExternalCommandBus>();

        services.TryAddScoped<IIdGenerator, NulloIdGenerator>();

        return services;
    }


    public static IServiceCollection AddTracing(this IServiceCollection services)
    {
        services.TryAddSingleton<ICorrelationIdFactory, GuidCorrelationIdFactory>();
        services.TryAddSingleton<ICausationIdFactory, GuidCausationIdFactory>();
        services.TryAddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();
        services.TryAddSingleton<ICausationIdProvider, CausationIdProvider>();
        services.TryAddSingleton<ITraceMetadataProvider, TraceMetadataProvider>();
        services.TryAddSingleton<ITracingScopeFactory, TracingScopeFactory>();

        services.TryAddScoped<Func<IServiceProvider, TraceMetadata?, TracingScope>>(sp =>
            (scopedServiceProvider, traceMetadata) =>
                sp.GetRequiredService<ITracingScopeFactory>().CreateTraceScope(scopedServiceProvider, traceMetadata)
        );

        services.TryAddScoped<Func<IServiceProvider, EventEnvelope?, TracingScope>>(sp =>
            (scopedServiceProvider, eventEnvelope) => sp.GetRequiredService<ITracingScopeFactory>()
                .CreateTraceScope(scopedServiceProvider, eventEnvelope)
        );

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        return services
            .AddScoped<IMediator, Mediator>()
            .AddTransient<ServiceFactory>(sp => sp.GetRequiredService!);
    }
}

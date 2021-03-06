using Core.Aggregates;
using Core.Events;
using System.Linq;

namespace Core.Testing;

public static class AggregateExtensions
{
    public static T? PublishedEvent<T>(this IAggregate aggregate) where T : class, IEvent
    {
        return aggregate.DequeueUncommittedEvents().LastOrDefault() as T;
    }
}

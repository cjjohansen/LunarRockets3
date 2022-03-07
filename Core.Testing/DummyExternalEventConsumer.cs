using Core.Events.External;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Testing;

public class DummyExternalEventConsumer: IExternalEventConsumer
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
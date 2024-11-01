using MassTransit;
using Microsoft.Extensions.Logging;

namespace BL;

public interface IQueueService
{
    Task PostToQueue<T>(T message, CancellationToken ct) where T : class;
}

public class QueueService(ILogger<QueueService> logger, IBus bus) : IQueueService
{
    public async Task PostToQueue<T>(T message, CancellationToken ct) where T : class
    {
        logger.LogInformation("[{name}] Posting to queue", typeof(T).Name);

        await bus.Publish(message, ct);
    }
}
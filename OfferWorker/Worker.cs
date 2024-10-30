using BL;

namespace OfferWorker;

public class Worker(ILogger<Worker> logger, IQueueService queueService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("Worker started...");
        try
        {
            Listen(0, ct);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not listen to queue");
        }

        return Task.CompletedTask;
    }

    private void Listen(int retries = 0, CancellationToken ct = default)
    {
        try
        {
            queueService.Listen(ct);
            logger.LogInformation("Listening to queue...");
        }
        catch (Exception e)
        {
            logger.LogWarning("Could not listen to queue: {e}", e.Message);
            Task.Delay(1000 * retries, ct).Wait(ct);

            if (retries >= 10)
                throw;

            Listen(retries + 1, ct);
        }
    }
}
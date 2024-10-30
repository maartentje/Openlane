using BL;

namespace OfferWorker;

public class Worker(ILogger<Worker> logger, IQueueService queueService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker started...");
        try
        {
            Listen();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not listen to queue");
        }

        return Task.CompletedTask;
    }

    private void Listen(int retries = 0)
    {
        try
        {
            queueService.Listen();
            logger.LogInformation("Listening to queue...");
        }
        catch (Exception e)
        {
            logger.LogWarning("Could not listen to queue: {e}", e.Message);
            Task.Delay(1000 * retries).Wait();

            if (retries >= 10)
                throw;

            Listen(retries + 1);
        }
    }
}
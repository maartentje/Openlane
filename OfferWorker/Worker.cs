using BL;

namespace OfferWorker;

public class Worker(ILogger<Worker> logger, IQueueService queueService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
        Listen();
        return Task.CompletedTask;
    }

    private void Listen()
    {
        try
        {
            queueService.Listen();
        }
        catch (Exception e)
        {
            logger.LogError("Error while starting worker (retrying): {e}", e.Message);
            Listen();
        }
    }
}
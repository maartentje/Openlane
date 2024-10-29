using BL;

namespace OfferWorker;

public class Worker(ILogger<Worker> logger, IOfferService offerService, IQueueService queueService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        queueService.Listen();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                var res = offerService.GetOffers();
                //logger.LogInformation("Worker running - name: {name} at: {time}", res.First().Id, DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
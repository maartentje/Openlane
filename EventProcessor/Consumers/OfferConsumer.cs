using BL;
using Domain.Model;
using MassTransit;

namespace EventProcessor.Consumers;

public class OfferConsumer(ILogger<OfferConsumer> logger, IOfferService offerService, ICarService carService) : IConsumer<Offer>
{
    public Task Consume(ConsumeContext<Offer> context)
    {
        logger.LogWarning("[] Consumed offer message");
        offerService.AddOffer(context.Message);
        return Task.CompletedTask;
    }
}
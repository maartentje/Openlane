using DAL;
using Domain.Model;
using Microsoft.Extensions.Logging;

namespace BL;

public interface IOfferService
{
    public void ProcessOffer(Offer offer);
}

public class OfferService(ILogger<OfferService> logger, IOfferRepository offerRepository) : IOfferService
{
    public void ProcessOffer(Offer offer)
    {
        logger.LogInformation("[{id}] Start processing", offer.Id);
        //could do anything here as 'processing'
        //for now we 'fake' the processing time
        Task.Delay(4_000).Wait();
        logger.LogInformation("[{id}] Processed", offer.Id);

        var existing = offerRepository.GetById(offer.Id);
        if (existing == null)
        {
            offerRepository.Create(offer);
            logger.LogInformation("[{id}] Created", offer.Id);
        }
        else
        {
            offerRepository.Update(offer);
            logger.LogInformation("[{id}] Updated", offer.Id);
        }
    }
}
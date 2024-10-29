using DAL;
using Domain;
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
        logger.LogInformation("Processing offer ({offer})", offer.Id);
        
        //could do anything here as 'processing' - for now, simple check + state change
        
        if (offer.State != State.Open || offer.Price < 100)
        {
            offer.State = State.Declined;
            logger.LogInformation("Offer declined ({offer})", offer.Id);
        }

        offerRepository.CreateOffer(offer);
        logger.LogInformation("Offer created ({offer})", offer.Id);
    }
}
using DAL;
using Domain.Model;
using Microsoft.Extensions.Logging;

namespace BL;

public interface IOfferService
{
    public Task ProcessOffer(Offer offer, CancellationToken ct);
}

public class OfferService(ILogger<OfferService> logger, IOfferRepository offerRepository) : IOfferService
{
    public async Task ProcessOffer(Offer offer, CancellationToken ct)
    {
        logger.LogInformation("[{id}] Start processing", offer.IdOutdated);
        //could do anything here as 'processing'
        //for now we 'fake' the processing time
        await Task.Delay(4_000, ct).WaitAsync(ct);
        logger.LogInformation("[{id}] Processed", offer.IdOutdated);

        var existing = await offerRepository.GetById(offer.IdOutdated, ct);
        if (existing == null)
        {
            await offerRepository.Create(offer, ct);
            logger.LogInformation("[{id}] Created", offer.IdOutdated);
        }
        else
        {
            await offerRepository.Update(offer, ct);
            logger.LogInformation("[{id}] Updated", offer.IdOutdated);
        }
    }
}
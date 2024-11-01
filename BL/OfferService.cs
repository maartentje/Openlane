using DAL;
using Domain.Model;
using Microsoft.Extensions.Logging;

namespace BL;

public interface IOfferService
{
    public void AddOffer(Offer offer, CancellationToken ct = default);
}

public class OfferService(ILogger<OfferService> logger, IBaseRepository<Offer> offerRepository) : IOfferService
{
    public void AddOffer(Offer offer, CancellationToken ct = default)
    {
        logger.LogInformation("[] Adding offer for car");
        offerRepository.Create(offer, ct);
    }
}
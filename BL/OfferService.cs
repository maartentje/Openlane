using DAL;
using Domain;

namespace BL;

public class OfferService(IOfferRepository offerRepository) : IOfferService
{
    public IEnumerable<Offer> GetOffers()
    {
        return offerRepository.ReadOffers();
    }
}
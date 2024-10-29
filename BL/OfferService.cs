using DAL;
using Domain;

namespace BL;

public interface IOfferService
{
    public IEnumerable<Offer> GetOffers();
}

public class OfferService(IOfferRepository offerRepository) : IOfferService
{
    public IEnumerable<Offer> GetOffers()
    {
        return offerRepository.ReadOffers();
    }
}
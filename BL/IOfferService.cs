using Domain;

namespace BL;

public interface IOfferService
{
    public IEnumerable<Offer> GetOffers();
}
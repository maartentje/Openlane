using Domain;

namespace DAL;

public interface IOfferRepository
{
    public IEnumerable<Offer> ReadOffers();
}
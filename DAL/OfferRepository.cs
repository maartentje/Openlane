using Domain;

namespace DAL;

public class OfferRepository : IOfferRepository
{
    public IEnumerable<Offer> ReadOffers()
    {
        return
        [
            new Offer
            {
                Id = "test-id",
                Price = 1.5d,
                State = State.New
            }
        ];
    }
}
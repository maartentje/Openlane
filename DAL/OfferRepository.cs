using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL;

public interface IOfferRepository
{
    public Offer CreateOffer(Offer offer);
}

public class OfferRepository(ILogger<OfferRepository> logger, IServiceProvider serviceProvider) : IOfferRepository
{
    public Offer CreateOffer(Offer offer)
    {
        logger.LogInformation("Creating offer");
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        dbContext.Offers.Add(offer);
        dbContext.SaveChanges();
        return offer;
    }
}
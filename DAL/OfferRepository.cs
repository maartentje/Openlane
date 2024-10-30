using Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL;

public interface IOfferRepository
{
    public Offer? GetById(string id);
    public void Create(Offer offer);
    public void Update(Offer offer);
}

public class OfferRepository(ILogger<OfferRepository> logger, IServiceProvider serviceProvider) : IOfferRepository
{
    public Offer? GetById(string id)
    {
        logger.LogInformation("[{id}] Get by id", id);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        return dbContext.Offers.Find(id);
    }

    public void Create(Offer offer)
    {
        logger.LogInformation("[{id}] Creating", offer.Id);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        dbContext.Offers.Add(offer);
        dbContext.SaveChanges();
    }

    public void Update(Offer offer)
    {
        logger.LogInformation("[{id}] Updating", offer.Id);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        dbContext.Offers.Update(offer);
        dbContext.SaveChanges();
    }
}
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL;

public interface IOfferRepository
{
    public Task<Offer?> GetById(string id, CancellationToken ct);
    public Task Create(Offer offer, CancellationToken ct);
    public Task Update(Offer offer, CancellationToken ct);
}

public class OfferRepository(ILogger<OfferRepository> logger, IServiceProvider serviceProvider) : IOfferRepository
{
    public async Task<Offer?> GetById(string id, CancellationToken ct)
    {
        logger.LogInformation("[{id}] Get by id", id);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        return await dbContext.Offers.FindAsync([id], cancellationToken: ct);
    }

    public async Task Create(Offer offer, CancellationToken ct)
    {
        logger.LogInformation("[{id}] Creating", offer.Id);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        await dbContext.Offers.AddAsync(offer, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task Update(Offer offer, CancellationToken ct)
    {
        logger.LogInformation("[{id}] Updating", offer.Id);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        dbContext.Entry(offer).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(ct);
    }
}
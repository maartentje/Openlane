using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DAL;

public interface IBaseRepository<T> where T : class
{
    public IQueryable<T> GetAll();
    public Task<T?> ReadById(Guid id, CancellationToken ct);
    public void Create(T entity, CancellationToken ct);
    public void Update(T entity, CancellationToken ct);
}

public class BaseRepository<T>(ILogger<BaseRepository<T>> logger, IServiceProvider serviceProvider)
    : IBaseRepository<T> where T : class
{
    public IQueryable<T> GetAll()
    {
        logger.LogInformation("[{name}] Reading all entities", typeof(T).Name);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        return dbContext.Set<T>().ToList().AsQueryable();
    }

    public async Task<T?> ReadById(Guid id, CancellationToken ct)
    {
        logger.LogInformation("[{name}] Reading entity", typeof(T).Name);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        return await dbContext.FindAsync<T>([id], ct);
    }

    public async void Create(T entity, CancellationToken ct)
    {
        logger.LogInformation("[{name}] Creating entity", typeof(T).Name);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        await dbContext.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async void Update(T entity, CancellationToken ct)
    {
        logger.LogInformation("[{name}] Updating entity", typeof(T).Name);
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(ct);
    }
}
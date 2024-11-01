using BL;
using DAL;
using Domain.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class RegisterSharedServices
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IQueueService, QueueService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IOfferService, OfferService>();
        
        services.AddScoped<IBaseRepository<Car>, BaseRepository<Car>>();
        services.AddScoped<IBaseRepository<Offer>, BaseRepository<Offer>>();
    }
}
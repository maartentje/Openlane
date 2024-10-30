using BL;
using DAL;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class RegisterSharedServices
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        services.AddTransient<IOfferService, OfferService>();
        services.AddTransient<IOfferRepository, OfferRepository>();
        services.AddSingleton<IQueueService, QueueService>();
    }
}
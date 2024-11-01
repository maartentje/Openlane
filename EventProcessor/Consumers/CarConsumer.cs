using BL;
using Domain.Model;
using MassTransit;

namespace EventProcessor.Consumers;

public class CarConsumer(ILogger<CarConsumer> logger, ICarService carService) : IConsumer<Car>
{
    public Task Consume(ConsumeContext<Car> context)
    {
        logger.LogWarning("[{name}] Consumed car message", context.Message.Title);
        carService.AddCar(context.Message);
        return Task.CompletedTask;
    }
}
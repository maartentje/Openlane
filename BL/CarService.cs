using DAL;
using Domain.Model;
using Microsoft.Extensions.Logging;

namespace BL;

public interface ICarService
{
    public Car? GetCarByTitle(string title);
    public void AddCar(Car car, CancellationToken ct = default);
    public void ChangeCar(Car car, CancellationToken ct = default);
}

public class CarService(ILogger<OfferService> logger, IBaseRepository<Car> carRepository) : ICarService
{
    public Car? GetCarByTitle(string title)
    {
        logger.LogInformation("[{title}] Getting car by title", title);
        var car = carRepository.GetAll().FirstOrDefault(c => c.Title == title);
        return car;
    }

    public void AddCar(Car car, CancellationToken ct)
    {
        logger.LogInformation("[{name}] Adding car", car.Title);
        carRepository.Create(car, ct);
    }
    
    public void ChangeCar(Car car, CancellationToken ct)
    {
        logger.LogInformation("[{name}] Updating car", car.Title);
        carRepository.Update(car, ct);
    }
}
using BL;
using Domain.Dto;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
public class ApiController(ILogger<ApiController> logger, IQueueService queueService, ICarService carService) : Controller
{
    [HttpPost("car")]
    public ActionResult CreateCar([FromBody] CarDto? dto, CancellationToken ct)
    {
        logger.LogInformation("Request received at '/api/CreateCar'");
        if (dto == null)
            return BadRequest("Invalid data");
        if (string.IsNullOrEmpty(dto.Title))
            return BadRequest("Invalid title");
        
        queueService.PostToQueue(new Car(dto), ct);
        return Ok();
    }
    
    [HttpPost("offer")]
    public ActionResult CreateOffer([FromBody] OfferDto? dto, CancellationToken ct)
    {
        logger.LogInformation("Request received at '/api/CreateOffer'");
        if (dto == null)
            return BadRequest("Invalid data");
        if (string.IsNullOrEmpty(dto.CarTitle))
            return BadRequest("Invalid car title");
        if (dto.Price < 100)
            return BadRequest("Invalid price: minimum value is 100");
        
        var car = carService.GetCarByTitle(dto.CarTitle);
        if (car == null)
            return BadRequest("Invalid car: could not find car");
        
        var offer = new Offer(dto)
        {
            CarId = car.Id,
            State = State.Open
        };
        
        queueService.PostToQueue(offer, ct);
        return Ok();
    }
}
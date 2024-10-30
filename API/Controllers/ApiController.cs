using BL;
using Domain.Dto;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
public class ApiController(ILogger<ApiController> logger, IQueueService queueService) : Controller
{
    [Route("sendMessage")]
    [HttpPost]
    public ActionResult SendMessage([FromBody] OfferDto? dto)
    {
        logger.LogInformation("Request received at '/api/sendMessage'");
        if (dto == null)
            return BadRequest("Invalid data");
        if (string.IsNullOrEmpty(dto.Id))
            return BadRequest("Invalid id");
        if (!Enum.IsDefined(typeof(State), dto.State))
            return BadRequest("Invalid state");

        var offer = new Offer
        {
            Id = dto.Id,
            Price = dto.Price,
            State = (State)dto.State,
        };

        logger.LogInformation("Send message ({id})", dto.Id);
        queueService.PostToQueue(offer);
        return Ok();
    }
}
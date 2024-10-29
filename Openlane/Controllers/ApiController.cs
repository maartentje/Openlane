using BL;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Openlane.Controllers;

[Route("[controller]")]
public class ApiController(ILogger<ApiController> logger, IQueueService queueService) : Controller
{
    [Route("sendMessage/{id}")]
    [HttpPost]
    public void SendMessage(string id)
    {
        logger.LogInformation("Send message ({id})", id);

        var offer = new Offer
        {
            Id = id,
            Price = 100.5d,
            State = State.Open,
        };
        
        queueService.PostToQueue(offer);
    }
}
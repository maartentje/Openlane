using BL;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Openlane.Pages;

public class IndexModel(ILogger<IndexModel> logger, IQueueService queueService) : PageModel
{
    public void OnGet()
    {
        logger.LogInformation("OnGet");
    }

    public void OnPostSendMessage()
    {
        logger.LogInformation("OnPostSendMessage");

        var offer = new Offer()
        {
            Id = "From UI-Portal",
            Price = 100.5d,
            State = State.Open,
        };
        
        queueService.PostToQueue(offer);
    }
}
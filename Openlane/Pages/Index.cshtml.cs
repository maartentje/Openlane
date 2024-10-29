using BL;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Openlane.Pages;

public class IndexModel(IQueueService queueService, ILogger<IndexModel> logger) : PageModel
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
            State = State.New,
        };
        
        queueService.PostToQueue(offer);
    }
}
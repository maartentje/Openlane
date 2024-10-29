using BL;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Openlane.Pages;

public class IndexModel(IOfferService offerService, ILogger<IndexModel> logger) : PageModel
{
    public void OnGet()
    {
        logger.LogInformation("OnGet");
        var res = offerService.GetOffers();
        ViewData["offers"] = res;
    }
}
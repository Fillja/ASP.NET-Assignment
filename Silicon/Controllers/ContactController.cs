using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class ContactController : Controller
{
    [Route("/contact")]
    public IActionResult Index()
    {
        return View();
    }
}

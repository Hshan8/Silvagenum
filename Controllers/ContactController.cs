using Microsoft.AspNetCore.Mvc;

namespace SilvagenumWebApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

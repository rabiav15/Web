using Microsoft.AspNetCore.Mvc;

namespace SalonYonetimUygulamasi.Controllers
{
    public class SalonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

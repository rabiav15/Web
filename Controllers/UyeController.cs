using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SalonYonetimUygulamasi.Controllers
{

	[Authorize(Roles = "Üye")]
	public class UyeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Profil()
		{
			// Üye profili işlemleri
			return View();
		}

		public IActionResult Randevularim()
		{
			// Üyenin randevuları
			return View();
		}
	}
}

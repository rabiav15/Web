using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SalonYonetimUygulamasi.Controllers
{



	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult SalonYonetimi()
		{
			// Kullanıcı yönetimi işlemleri
			return View();

		}
		public IActionResult CalisanYonetimi()
		{
			// Raporlama işlemleri
			return View();
		}
	}


}
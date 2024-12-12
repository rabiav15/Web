using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;

namespace SalonYonetimUygulamasi.Controllers
{
	[Authorize(Roles = "Üye")]
	public class UyeController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;


		public UyeController(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		// GET: /Uye/Dashboard
		public async Task<IActionResult> Dashboard()
		{
			var user = await _userManager.GetUserAsync(User);
			var model = new UyeDashboardView
			{
				Email = user.Email,
				Randevular = new List<Randevu>() // Burada randevularınızı çekip doldurun
			};

			return View(model);
		}

		public IActionResult Index()
		{
			return View();

		}
		public IActionResult Profil()
		{
			return View();
		}
		public async Task<IActionResult> Randevularim()
		{    

			var user = await _userManager.GetUserAsync(User);

			// Kullanıcının randevularını çekmek için gerekli işlemleri yapın
			var randevular = new List<Randevu>(); // Burada veritabanından randevuları çekin

			return View(randevular); // Randevularim.cshtml view'ine randevuları gönder
		} 
		


	}
}
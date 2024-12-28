using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAI;
using SalonYonetimUygulamasi.Models;

namespace SalonYonetimUygulamasi.Controllers
{
	[Authorize(Roles = "Üye")]
	public class UyeController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SalonContext _context;

		public UyeController(UserManager<IdentityUser> userManager , SalonContext context)
		{
			_context = context;
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
		
		public IActionResult UyeIslemGor()
		{
			var islemler = _context.Islemler
	.Include(i => i.Calisan)
	.ToList();
			return View(islemler);
			
		}

	}
}
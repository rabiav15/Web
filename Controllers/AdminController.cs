using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SalonYonetimUygulamasi.Models;

namespace SalonYonetimUygulamasi.Controllers
{

	[Authorize(Roles = "Admin")]
	[Route("Admin")]
	public class AdminController : Controller
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;

		public AdminController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		[HttpGet("Login")]
		public IActionResult Login()
		{
			return View();
		}
		[AllowAnonymous]
		[HttpPost("Login")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(AdminLoginView model)
		{
			if (!ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					ModelState.AddModelError("", "Kullanıcı bulunamadı.");
					return View(model);
				}

				var roles = await _userManager.GetRolesAsync(user);
				if (!roles.Contains("Admin"))
				{
					ModelState.AddModelError("", "Bu kullanıcı Admin rolüne sahip değil.");
					return View(model);
				}

				var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
				if (result.Succeeded)
				{
					return RedirectToAction("Dashboard", "Admin");
				}

				ModelState.AddModelError("", "Geçersiz giriş bilgileri.");
			}

			return View(model);
		}

		[HttpGet("Dashboard")]
		public IActionResult Dashboard()
		{
			// Dashboard'a özel veriler burada hazırlanabilir
			var model = new AdminDashboardView
			{
				TotalUsers = 100, // Örnek değer
				TotalRandevu = 50, // Örnek değer
				TotalCalisanlar = 10 // Örnek değer
			};

			return View(model);
		}

		[HttpGet("Index")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("Logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync(); // Oturumu kapat
			return RedirectToAction("Login", "Admin"); // Admin giriş sayfasına yönlendir
		}

	}
}
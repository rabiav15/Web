using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using SalonYonetimUygulamasi.Models;

namespace SalonYonetimUygulamasi.Controllers
{


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

		// GET: Admin/Login
		[HttpGet("Login")]
		public IActionResult Login()
		{
			return View();
		}

		// POST: Admin/Login
		[HttpPost("Login")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(AdminLoginView model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					ModelState.AddModelError("", "Geçersiz kullanıcı bilgileri.");
					return View(model);
				}

				var roles = await _userManager.GetRolesAsync(user);
				if (!roles.Contains("Admin"))
				{
					ModelState.AddModelError("", "Yalnızca Admin yetkisi olan kullanıcılar giriş yapabilir.");
					return View(model);
				}

				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					return RedirectToAction("Dashboard", "Admin");
				}

				ModelState.AddModelError("", "Giriş başarısız. Lütfen bilgilerinizi kontrol edin.");
			}

			return View(model);
		}

		// GET: Admin/Dashboard
		[HttpGet("Dashboard")]
		public IActionResult Dashboard()
		{
			return View();
		}

		// GET: Admin/Logout
		[HttpGet("Logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Admin");
		}
	}
}
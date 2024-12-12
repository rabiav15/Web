using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalonYonetimUygulamasi.Models;
using System.Threading.Tasks;

public class AccountController : Controller
{

	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;

	public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
	{
		_userManager = userManager;
		_signInManager = signInManager;
	}
		// GET: /Account/Login
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		// POST: /Account/Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginView model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					var user = await _userManager.FindByEmailAsync(model.Email);
					var roles = await _userManager.GetRolesAsync(user);

					// Üye kontrolü
					if (roles.Contains("Üye"))
					{
						return RedirectToAction("Dashboard", "Uye");
					}
				}

				ModelState.AddModelError("", "Geçersiz giriş bilgileri.");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		// POST: Account/Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterView model)
		{
			if (!ModelState.IsValid)
			{
				return View(model); // Model geçersizse formu tekrar göster
			}

			var user = new IdentityUser
			{
				UserName = model.Email,
				Email = model.Email
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, "Üye"); // Kullanıcıya Üye rolü atanıyor
				await _signInManager.SignInAsync(user, isPersistent: false); // Kullanıcı oturum açıyor
				return RedirectToAction("Dashboard", "Uye");
			}

			// Hataları göster
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(model); // Başarısız olduğunda formu tekrar göster
		}


		[HttpGet]
		public IActionResult Logout()
		{
			return View();
		}

		// POST: Account/Logout
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogoutConfirmed()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}
	}


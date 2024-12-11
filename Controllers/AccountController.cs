using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
	public async Task<IActionResult> Login(string email, string password, bool rememberMe)
	{
		var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

		if (result.Succeeded)
		{
			// Giriş başarılı, ana sayfaya yönlendir
			return RedirectToAction("Index", "Home");
		}

		ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
		return View();
	}

	// GET: /Account/Logout
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Logout()
	{
		await _signInManager.SignOutAsync();
		return RedirectToAction("Index", "Home");
	}
}

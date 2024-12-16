using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SalonYonetimUygulamasi.Models
{
	public static class SeedRolesAndUsers
	{
		public static async Task SeedRolesUsers(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

			// Admin rolünü oluştur
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				await roleManager.CreateAsync(new IdentityRole("Admin"));
			}

			// Üye rolünü oluştur
			if (!await roleManager.RoleExistsAsync("Üye"))
			{
				await roleManager.CreateAsync(new IdentityRole("Üye"));
			}

			// Admin kullanıcısını oluştur
			var adminUser = await userManager.FindByEmailAsync("admin@example.com");
			if (adminUser == null)
			{
				adminUser = new IdentityUser
				{
					UserName = "admin",
					Email = "admin@example.com",
					EmailConfirmed = true // E-posta doğrulandı olarak işaretle
				};

				var result = await userManager.CreateAsync(adminUser, "Admin123!");
				if (result.Succeeded)
				{
					// Admin rolünü ata
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
				else
				{
					// Hata durumunda loglama
					foreach (var error in result.Errors)
					{
						Console.WriteLine($"Admin oluşturulamadı: {error.Description}");
					}
				}
			}
		}
	}
}

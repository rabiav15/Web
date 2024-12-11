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

			Console.WriteLine("SeedRolesAndUsers: Başlıyor");

			// Admin rolü oluşturma
			if (!await roleManager.RoleExistsAsync("Admin"))
			{
				Console.WriteLine("Admin rolü oluşturuluyor");
				await roleManager.CreateAsync(new IdentityRole("Admin"));
			}

			// Üye rolü oluşturma
			if (!await roleManager.RoleExistsAsync("Üye"))
			{
				Console.WriteLine("Üye rolü oluşturuluyor");
				await roleManager.CreateAsync(new IdentityRole("Üye"));
			}

			// Admin kullanıcısı oluşturma
			var adminUser = await userManager.FindByEmailAsync("admin@example.com");
			if (adminUser == null)
			{
				Console.WriteLine("Admin kullanıcı oluşturuluyor");
				adminUser = new IdentityUser
				{
					UserName = "admin",
					Email = "admin@example.com",
					EmailConfirmed = true
				};
				await userManager.CreateAsync(adminUser, "Admin123!");
				await userManager.AddToRoleAsync(adminUser, "Admin");
			}

			Console.WriteLine("SeedRolesAndUsers: Tamamlandı");

		}


	}
}

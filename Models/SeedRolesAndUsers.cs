﻿using Microsoft.AspNetCore.Identity;
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
        var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!roleResult.Succeeded)
        {
            Console.WriteLine("Admin rolü oluşturulamadı:");
            foreach (var error in roleResult.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }

    // Üye rolü oluşturma
    if (!await roleManager.RoleExistsAsync("Üye"))
    {
        var roleResult = await roleManager.CreateAsync(new IdentityRole("Üye"));
        if (!roleResult.Succeeded)
        {
            Console.WriteLine("Üye rolü oluşturulamadı:");
            foreach (var error in roleResult.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }

    // Admin kullanıcısı oluşturma
    var adminUser = await userManager.FindByEmailAsync("b241210352@sakarya.edu.tr");
    if (adminUser == null)
    {
        Console.WriteLine("Admin kullanıcı oluşturuluyor");
        adminUser = new IdentityUser
        {
            UserName = "admin",
            Email = "b241210352@sakarya.edu.tr",
            EmailConfirmed = true
        };

        var userResult = await userManager.CreateAsync(adminUser, "varolRabia-150912");
        if (userResult.Succeeded)
        {
            var roleAssignResult = await userManager.AddToRoleAsync(adminUser, "Admin");
            if (!roleAssignResult.Succeeded)
            {
                Console.WriteLine("Admin rolü atanamadı:");
                foreach (var error in roleAssignResult.Errors)
                {
                    Console.WriteLine(error.Description);
                }
            }
        }
        else
        {
            Console.WriteLine("Admin kullanıcı oluşturulamadı:");
            foreach (var error in userResult.Errors)
            {
                Console.WriteLine(error.Description);
            }
        }
    }

    Console.WriteLine("SeedRolesAndUsers: Tamamlandı");
}

	}
}
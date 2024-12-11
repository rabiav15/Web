using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

var connStr = "Server=(localdb)\\mssqllocaldb;Database=SalonYonetimUygulamasi;Trusted_Connection=True;";

builder.Services.AddDbContext<SalonContext>(
    options => options.UseSqlServer(connStr));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<SalonContext>()
	.AddDefaultTokenProviders();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		// Rolleri ve kullanýcýlarý tekrar zorla
		await SeedRolesAndUsers.SeedRolesUsers(services);
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Seed iþlemi sýrasýnda hata oluþtu: {ex.Message}");
	}
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

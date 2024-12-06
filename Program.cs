using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

	


var connStr = "Server=(localdb)\\mssqllocaldb;Database=SalonYonetimUygulamasi;Trusted_Connection=True;";

builder.Services.AddDbContext<SalonContext>(
    options => options.UseSqlServer(connStr));



var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

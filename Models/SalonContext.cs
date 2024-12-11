using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;
using System.Collections.Generic;

namespace SalonYonetimUygulamasi.Models
{
	public class SalonContext : IdentityDbContext<IdentityUser, IdentityRole, string>
	{
		public DbSet<Salon> Salonlar { get; set; }
		public DbSet<Calisan> Calisanlar { get; set; }

		public DbSet<Randevu> Randevular { get; set; }

		public DbSet<Islem> Islemler { get; set; }

		public SalonContext(DbContextOptions<SalonContext> options) : base(options) { }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Islem ile Calisan arasında ilişki tanımlıyoruz
			modelBuilder.Entity<Islem>()
				.HasOne(i => i.Calisan)
				.WithMany(c => c.Islemler)
				.HasForeignKey(i => i.CalisanID)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}

}
       

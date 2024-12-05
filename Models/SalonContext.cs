using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;
using System.Collections.Generic;

namespace SalonYonetimUygulamasi.Models
{
    public class SalonContext : DbContext
    {

        public SalonContext(DbContextOptions<SalonContext> options): base(options) { }
        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }
       

    }
}


       

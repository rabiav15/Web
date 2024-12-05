﻿
using System.ComponentModel.DataAnnotations;

namespace SalonYonetimUygulamasi.Models
{
    public class Calisan
    {
        [Required]

        public int CalisanID { get; set; }

        [Required]

        public string CalisanAd { get;  set; }

        [Required]

        public string CalisanSoyad { get; set; }

        [Required]

        public string UzmanlikAlani { get; set; }


        public int SalonID { get; set; }
        public Salon Salon { get; set; }
    }
}

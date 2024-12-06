using System;
using System.ComponentModel.DataAnnotations;
namespace SalonYonetimUygulamasi.Models
    
{
    public class Randevu
    {
        
        [Required]
            public int CalısanID { get; set; }
        [Required]

        public int RandevuID { get; set; }
        public DateTime Tarih { get; set; }
        [Required]
            public string Islem { get; set; }   
        [Required]
            public double Ucret { get; set; }

            // İlişki: Randevu bir çalışana bağlıdır.
            public int CalisanID { get; set; }
            public Calisan Calisan { get; set; }
        }
    }


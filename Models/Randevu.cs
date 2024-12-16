using System;
using System.ComponentModel.DataAnnotations;
namespace SalonYonetimUygulamasi.Models
    
{
    public class Randevu
    {

	
        [Required]

        public int RandevuID { get; set; }

        public DateTime Tarih {  get; set; }
     
        public TimeSpan Saat{get; set; }

      
 
		public double IslemUcreti { get; set; }
		public int IslemID { get; set; }
		// İlişki: Randevu bir çalışana bağlıdır.
		public int CalisanID { get; set; }
        public Calisan Calisan { get; set; }

        public Islem Islem { get; set; }
        }
    }


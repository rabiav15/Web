
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalonYonetimUygulamasi.Models
{
    public class Calisan
    {
		[Key] 
        public int CalisanID { get; set; }

        [Required]

        public string CalisanAd { get;  set; }

        [Required]

        public string CalisanSoyad { get; set; }

        [Required]

        public string UzmanlikAlani { get; set; }


       

		public Salon Salon { get; set; }



		[JsonIgnore]
		public virtual ICollection<Islem> Islemler { get; set; }
	}
	}

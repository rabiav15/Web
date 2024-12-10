using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace SalonYonetimUygulamasi.Models
{
    public class Salon
    {
        

        public int SalonID { get; set; }
        
		[Required(ErrorMessage = "Salon adı zorunludur.")]
		public string SalonAd { get; set; }
		[Required(ErrorMessage = "Salon adresi zorunludur.")]
		public string SalonAdres { get; set; }
		[Required(ErrorMessage = "Salon telefon numarası zorunludur.")]
		public string SalonTelefon { get; set; }

	

		public ICollection<Calisan> Calisanlar { get; set; }
    }
}

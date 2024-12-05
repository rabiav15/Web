using System.ComponentModel.DataAnnotations;

namespace SalonYonetimUygulamasi.Models
{
    public class Salon
    {
        [Required]

        public int SalonID { get; set; }
        [Required]

        public string SalonAd { get; set; }
        [Required]

        public string SalonAdres { get; set; }
        [Required]

        public string SalonTelefon { get; set; }

     
        public ICollection<Calisan> Calisanlar { get; set; }
    }
}

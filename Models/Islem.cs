namespace SalonYonetimUygulamasi.Models
{
	public class Islem
	{
		public int IslemID { get; set; }       // İşlem ID
		public string IslemAdi { get; set; }   // İşlem Adı
		public double IslemUcreti { get; set; } // İşlem Ücreti
		public int IslemSuresi { get; set; }   // İşlem Süresi (dakika)
		public int CalisanID { get; set; }     // Çalışan ID (Foreign Key)

		// Navigation Property
		public virtual Calisan Calisan { get; set; } 
		// İlgili Çalışan
	}

}

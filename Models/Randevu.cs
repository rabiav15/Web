using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SalonYonetimUygulamasi.Models

{
	
		public class Randevu
		{

			[Key]
			public int RandevuID { get; set; } // Randevu ID

			[Required]
			public int UserId { get; set; } // Randevu alan üye

			[Required]
			public int SalonID { get; set; } // Salon ID

			[Required]
			public int CalisanID { get; set; } // Çalışan ID

			[Required]
			public int IslemID { get; set; } // Hizmet ID

			[Required]
			[Column(TypeName = "date")]
			public DateTime Tarih { get; set; } // Randevu Tarihi

			[Required]
			[Column(TypeName = "time")]
			public TimeSpan Saat { get; set; } // Randevu Saati

			public double IslemUcreti { get; set; } // İşlem Ücreti

			public bool IsApproved { get; set; } = false; // Onay Durumu

			[Required]
			public DateTime CreatedAt { get; set; } = DateTime.Now; // Oluşturulma Zamanı

			// İlişkiler
			[ForeignKey("UserId")]
			public User Kullanici { get; set; } // Kullanıcı ile ilişki

			[ForeignKey("SalonId")]
			public Salon Salon { get; set; } // Salon ile ilişki

			[ForeignKey("CalisanID")]
			public Calisan Calisan { get; set; } // Çalışan ile ilişki

			[ForeignKey("IslemID")]
			public Islem Islem { get; set; } // Hizmet ile ilişki
		public virtual IdentityUser User { get; set; }
	}
	}

	


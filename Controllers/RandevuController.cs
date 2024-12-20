﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Migrations;
using SalonYonetimUygulamasi.Models;
using Randevu = SalonYonetimUygulamasi.Models.Randevu;



namespace SalonYonetimUygulamasi.Controllers
{
	
	public class RandevuController : Controller
	{
		
		private readonly SalonContext _context;
		

		public RandevuController(SalonContext context)
		{
			
			_context = context;
		}
		
		public IActionResult Index()
		{

			var randevular = _context.Randevular.Include(r => r.Calisan).Include(r=> r.Islem).ToList();
			return View(randevular);
		}


		// GET: Randevu/Create
		public IActionResult RandevuCreate()
		{
			try
			{
				// Tarih seçeneklerini oluşturuyoruz
				var dates = new List<string>();
				var now = DateTime.Now.Date; // Bugünün tarihi
				for (int i = 0; i < 7; i++)
				{
					dates.Add(now.AddDays(i).ToString("yyyy-MM-dd"));
				}
				ViewBag.Dates = dates;

				// Saat seçeneklerini oluşturuyoruz
				var allHours = new List<string>
		{
			"09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
			"12:00", "13:30", "14:00", "14:30", "15:00", "15:30",
			"16:00", "16:30"
		};

				// Mevcut randevuların saatlerini alıyoruz
				var takenHours = _context.Randevular
					.Select(r => r.Tarih.ToString("HH:mm"))
					.ToList();

				// Alınmamış saatleri belirliyoruz
				ViewBag.Hours = allHours.Where(hour => !takenHours.Contains(hour)).ToList();

				// Çalışanlar ve işlemleri ViewBag ile gönderiyoruz
				ViewBag.Calisanlar = _context.Calisanlar.ToList();
				ViewBag.Islemler = _context.Islemler.ToList();

				return View();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Hata: {ex.Message}");
				return View("Error"); // Hata sayfasına yönlendir
			}
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult RandevuCreate(string TarihDate, string TarihTime, Randevu randevu)
		{
			try
			{
				// Tarih ve saat birleştirme
				if (!string.IsNullOrEmpty(TarihDate) && !string.IsNullOrEmpty(TarihTime))
				{
					var selectedDate = DateTime.Parse(TarihDate);
					var selectedTime = TimeSpan.Parse(TarihTime);
					randevu.Tarih = selectedDate + selectedTime;

					// Aynı çalışandan aynı tarihte ve saatte randevu olup olmadığını kontrol et
					var mevcutRandevu = _context.Randevular
						.FirstOrDefault(r => r.CalisanID == randevu.CalisanID && r.Tarih == randevu.Tarih);

					if (mevcutRandevu != null)
					{
						ModelState.AddModelError("Tarih", "Bu çalışandan bu saat için zaten bir randevu alınmış.");
					}
				}
				else
				{
					ModelState.AddModelError("Tarih", "Tarih ve saat alanları doldurulmalıdır.");
				}

				// Seçilen işlem ID'sini kullanarak işlem ücretini alıyoruz
				var selectedIslem = _context.Islemler.FirstOrDefault(i => i.IslemID == randevu.IslemID);
				if (selectedIslem != null)
				{
					randevu.IslemUcreti = selectedIslem.IslemUcreti;
				}
				else
				{
					ModelState.AddModelError("IslemID", "Geçersiz bir işlem seçildi.");
				}

				// Model doğrulama ve kaydetme
				if (ModelState.IsValid)
				{
					_context.Randevular.Add(randevu);
					_context.SaveChanges();
					return RedirectToAction("Randevularim", "Uye");
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
			}

			// Sayfa yeniden yüklendiğinde gerekli ViewBag verilerini tekrar yükleyelim
			ViewBag.Dates = new List<string>();
			ViewBag.Hours = new List<string>();
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = _context.Islemler.ToList();

			return View(randevu);
		}

		public IActionResult RandevuDetails(int id)
		{
			var randevu = _context.Randevular
		.Include(r => r.Calisan) // Çalışan bilgisi dahil ediliyor
		.Include(r => r.Islem) // İşlem bilgisi dahil ediliyor
		.FirstOrDefault(r => r.RandevuID == id);

			if (randevu == null)
			{
				return NotFound(); // Eğer randevu bulunamazsa, 404 döner.
			}

			return View(randevu);
		}




		public IActionResult RandevuDelete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");  // Eğer ID null ise, anasayfaya yönlendir
			}

			var randevu = _context.Randevular.FirstOrDefault(c => c.RandevuID == id);

			if (randevu == null)
			{
				return RedirectToAction("Index");  // Eğer randevu bulunmazsa, anasayfaya yönlendir
			}

			// Silme onayı sayfasına yönlendir
			return View(randevu);
		}
		[HttpPost, ActionName("RandevuDelete")]
		[ValidateAntiForgeryToken]
		public IActionResult RandevuDeleteConfirmed(int id)
		{
			var randevu = _context.Randevular.FirstOrDefault(c => c.RandevuID == id);

			if (randevu == null)
			{
				return RedirectToAction("Index");  // Eğer randevu bulunamazsa, anasayfaya yönlendir
			}

			_context.Randevular.Remove(randevu);  // Randevuyu sil
			_context.SaveChanges();  // Değişiklikleri kaydet

			return RedirectToAction("Index");  // Silme işleminden sonra anasayfaya yönlendir
		}

	}


}


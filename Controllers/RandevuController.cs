using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;

using System.Text.Json.Serialization;
using System.Text.Json;
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

		private List<string> GetAvailableHours()
		{
			return new List<string>
	{
		"09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
		"12:00", "13:30", "14:00", "14:30", "15:00", "15:30",
		"16:00", "16:30"
	};
		}

		public IActionResult RandevuCreate()
		{
			// Şu anki tarih ile 7 gün sonraya kadar olan tarihleri alıyoruz
			ViewBag.Dates = Enumerable.Range(0, 7)
	.Select(i => DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"))
	.ToList();


			// Saatleri ekliyoruz
			ViewBag.Hours = GetAvailableHours();

			// Çalışanlar ve işlemler verilerini ViewBag'e ekliyoruz
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = _context.Islemler.ToList();

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult RandevuCreate(string TarihDate, string TarihTime, Randevu randevu)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.Dates = Enumerable.Range(0, 7)
					.Select(i => DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"))
					.ToList();
				ViewBag.Hours = GetAvailableHours();
				ViewBag.Calisanlar = _context.Calisanlar.ToList();
				ViewBag.Islemler = _context.Islemler.ToList();
				return View(randevu);
			}

			// Debugging: Tarih ve saat bilgilerini kontrol et
			Console.WriteLine($"Tarih: {TarihDate}, Saat: {TarihTime}");

			if (!string.IsNullOrEmpty(TarihDate) && !string.IsNullOrEmpty(TarihTime))
			{
				try
				{
					// Tarih ve saati birleştirerek DateTime türünde bir değer oluşturuyoruz
					var randevuTarihi = DateTime.Parse($"{TarihDate} {TarihTime}");
					randevu.Tarih = randevuTarihi;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Hata: {ex.Message}");
				}
			}

			_context.Randevular.Add(randevu);
			_context.SaveChanges();

			return RedirectToAction("Index");
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



		[Authorize(Roles = "Admin")]
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


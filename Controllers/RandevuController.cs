using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;


using System.Text.Json.Serialization;
using System.Text.Json;
using Randevu = SalonYonetimUygulamasi.Models.Randevu;
using SalonYonetimUygulamasi.Migrations;
using Newtonsoft.Json;



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
			var randevular = _context.Randevular
				.Include(r => r.Calisan)
				.Include(r => r.Islem)
				.Include(r => r.Salon)  // Salon ilişkisini ekliyoruz
				   // Kullanıcı ilişkisini ekliyoruz (user için ilişkili ise)
				.ToList();

			return View(randevular);
		}

		private List<string> GetAvailableHours()
		{
			return Enumerable.Range(9, 12) // 09:00 ile 20:00 arasındaki saatler
							 .Select(h => $"{h:00}:00")
							 .ToList();
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
			ViewBag.Salonlar = _context.Salonlar.ToList(); // Salonlar ekleniyor
			ViewBag.Users = _context.Users.ToList(); // Kullanıcılar ekleniyor (admin veya üye rolü)

			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult RandevuCreate(string TarihDate, string TarihTime, Randevu randevu)
		{
			var settings = new JsonSerializerSettings
			{
				MaxDepth = 100, // Max depth'i artırın (gerekirse)
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};

			// JSON'e çevirme işlemi
			var json = JsonConvert.SerializeObject(randevu, settings);
			if (!ModelState.IsValid)
			{
				// Gerekli bilgileri tekrar ViewBag'e ekliyoruz
				ViewBag.Dates = Enumerable.Range(0, 7)
					.Select(i => DateTime.Now.AddDays(i).ToString("yyyy-MM-dd"))
					.ToList();
				ViewBag.Hours = GetAvailableHours();
				ViewBag.Calisanlar = _context.Calisanlar.ToList();
				ViewBag.Islemler = _context.Islemler.ToList();
				ViewBag.Salonlar = _context.Salonlar.ToList();
				return View(randevu);
			}

			// Tarih ve saat bilgilerini kontrol et
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
				.Include(r => r.Calisan)
				.Include(r => r.Islem)
				.Include(r => r.Salon)
				
				.FirstOrDefault(r => r.RandevuID == id);

			if (randevu == null)
			{
				return NotFound();
			}

			return View(randevu);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult RandevuDelete(int? id)
		{
			if (id == null)
			{
				return RedirectToAction("Index");
			}

			var randevu = _context.Randevular.FirstOrDefault(c => c.RandevuID == id);

			if (randevu == null)
			{
				return RedirectToAction("Index");
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
				return RedirectToAction("Index");
			}

			_context.Randevular.Remove(randevu);  // Randevuyu sil
			_context.SaveChanges();  // Değişiklikleri kaydet

			return RedirectToAction("Index");
		}

		

}
}

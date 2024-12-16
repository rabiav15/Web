using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;



namespace SalonYonetimUygulamasi.Controllers
{
	
	public class RandevuController : Controller
	{
		
		private readonly SalonContext _context;
		

		public RandevuController(SalonContext context)
		{
			
			_context = context;
		}
		[Authorize(Roles = "Admin")]
		public IActionResult Index()
		{

			var randevular = _context.Randevular.Include(r => r.Calisan).ToList();
			return View(randevular);
		}


		// GET: Randevu/Create
		public IActionResult RandevuCreate()
		{
			// Önümüzdeki 7 günün tarihlerini ViewBag'e gönderiyoruz
			var dates = new List<string>();
			var now = DateTime.Now.Date; // Bugünün tarihi
			for (int i = 0; i < 7; i++)
			{
				dates.Add(now.AddDays(i).ToString("yyyy-MM-dd"));
			}
			ViewBag.Dates = dates;

			// Saatler (sabit saatler listesi)
			var allHours = new List<string>
	{
		"09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
		"12:00", "13:30", "14:00", "14:30", "15:00", "15:30",
		"16:00", "16:30"
	};

			// Randevu alınmış saatleri filtreleyelim
			var takenHours = _context.Randevular
				.Where(r => r.CalisanID == 1) // Eğer çalışan id varsa, onu da filtreleyin
				.Select(r => r.Tarih.ToString("HH:mm"))
				.ToList();

			// Alınmamış saatleri filtreleyelim
			var availableHours = allHours.Where(hour => !takenHours.Contains(hour)).ToList();

			// ViewBag'e alınmış saatleri atıyoruz
			ViewBag.Hours = availableHours;

			// Çalışanlar ve işlemler veritabanından çekiliyor
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = _context.Islemler.ToList(); // İşlemleri dinamik olarak alıyoruz

			return View();
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
					var selectedDate = DateTime.Parse(TarihDate);  // Tarih bilgisini al
					var selectedTime = TimeSpan.Parse(TarihTime);  // Saat bilgisini al
					randevu.Tarih = selectedDate + selectedTime;  // Tarih ve saati birleştir

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
					randevu.IslemUcreti = selectedIslem.IslemUcreti;  // İşlem ücretini dinamik olarak ata
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
			ViewBag.Dates = new List<string>();  // Gerektiği şekilde
			ViewBag.Hours = new List<string>();
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = _context.Islemler.ToList();  // İşlemleri tekrar yükleyelim

			return View(randevu);
		}



		public IActionResult RandevuDetails(int id)
		{
			var randevu = _context.Randevular
				.Include(r => r.Calisan)
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


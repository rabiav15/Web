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
		public IActionResult Index()
		{

			var randevular = _context.Randevular.Include(r => r.Calisan).ToList();
			return View(randevular);
		}


		public IActionResult RandevuCreate()
		{
			// Önümüzdeki 7 günün tarihlerini ViewBag'e gönderiyoruz.
			var dates = new List<string>();
			var now = DateTime.Now.Date; // Bugünün tarihi
			for (int i = 0; i < 7; i++)
			{
				dates.Add(now.AddDays(i).ToString("yyyy-MM-dd"));
			}
			ViewBag.Dates = dates;

			// Saat seçenekleri
			ViewBag.Hours = new List<string>
			{
				"09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
				"12:00", "13:30", "14:00", "14:30", "15:00", "15:30",
				"16:00", "16:30"
			};

			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = new[] { "Makyaj", "Saç" };
			return View();
		}

		// POST: Randevu/Create
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

				// İşlem bazlı ücret atama
				if (randevu.Islem == "Makyaj")
				{
					randevu.Ucret = 500;
				}
				else if (randevu.Islem == "Saç")
				{
					randevu.Ucret = 750;
				}
				else
				{
					ModelState.AddModelError("Islem", "Geçersiz işlem.");
				}

				// Model doğrulama ve kaydetme
				if (!ModelState.IsValid)
				{
					_context.Randevular.Add(randevu);
					_context.SaveChanges();
					return RedirectToAction(nameof(Index));
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
			}

			// Tarih seçeneklerini oluşturuyoruz (Bugünden itibaren 7 gün)
			var dates = new List<string>();
			var now = DateTime.Now.Date;
			for (int i = 0; i < 7; i++)
			{
				dates.Add(now.AddDays(i).ToString("yyyy-MM-dd"));
			}
			ViewBag.Dates = dates;

			// Saat seçeneklerini oluşturuyoruz ve zaten alınmış saatleri filtreliyoruz
			var allHours = new List<string>
	{
		"09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
		"12:00", "13:30", "14:00", "14:30", "15:00", "15:30",
		"16:00", "16:30"
	};

			// Randevu alınmış saatleri filtreleyelim
			var takenHours = _context.Randevular
				.Where(r => r.CalisanID == randevu.CalisanID && r.Tarih.Date == randevu.Tarih.Date)
				.Select(r => r.Tarih.ToString("HH:mm"))
				.ToList();

			var availableHours = allHours.Where(hour => !takenHours.Contains(hour)).ToList();

			ViewBag.Hours = availableHours;
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = new[] { "Makyaj", "Saç" };

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

		public IActionResult RandevuEdit(int id)
		{
			var randevu = _context.Randevular
				.Include(r => r.Calisan)
				.FirstOrDefault(r => r.RandevuID == id);

			if (randevu == null)
			{
				return NotFound(); // Eğer randevu bulunamazsa, 404 döner.
			}

			// Çalışanlar ve işlem türleri seçeneklerini ViewBag'e gönderiyoruz
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = new[] { "Makyaj", "Saç" }; // İşlem türleri

			return View(randevu);
		}

		// POST: Randevu/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult RandevuEdit(int id, Randevu randevu)
		{
			if (id != randevu.RandevuID)
			{
				return NotFound(); // Eğer ID'ler eşleşmezse, 404 döner
			}

			// Aynı saatte aynı çalışandan randevu alınamamalı
			var mevcutRandevu = _context.Randevular
				.FirstOrDefault(r => r.CalisanID == randevu.CalisanID && r.Tarih == randevu.Tarih && r.RandevuID != randevu.RandevuID);

			if (mevcutRandevu != null)
			{
				ModelState.AddModelError("Tarih", "Bu çalışandan bu saatte başka bir randevu bulunmaktadır.");
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Entry(randevu).State = EntityState.Modified; // Mevcut randevuyu güncelliyoruz
					_context.SaveChanges();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Randevular.Any(r => r.RandevuID == randevu.RandevuID))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				return RedirectToAction(nameof(Index)); // Kaydettikten sonra anasayfaya dön
			}

			// Hata durumunda, seçenekleri yeniden gönderiyoruz
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = new[] { "Makyaj", "Saç" }; // İşlem türleri
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


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;

namespace SalonYonetimUygulamasi.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CalisanController : Controller
	{

		private readonly SalonContext _context;


		public CalisanController(SalonContext context)
		{

			_context = context;
		}



		public IActionResult Index()
		{
			var calisanlar = _context.Calisanlar.Include(c => c.Salon).ToList();
			return View(calisanlar);
		}
		// Create: Show the form to add a new employee
		public IActionResult CalisanEkle()
		{
			var salonlar = _context.Salonlar.ToList();

			// Eğer salonlar varsa, ViewBag'e salonlar listesini ekliyoruz
			if (salonlar != null && salonlar.Any())
			{
				ViewBag.Salonlar = salonlar;
			}
			else
			{
				// Eğer salonlar yoksa boş bir liste atıyoruz
				ViewBag.Salonlar = new List<Salon>();
			}

			return View();
		}
		[HttpPost]
		public IActionResult CalisanEkle(Calisan c)
		{

			if (ModelState.IsValid)
			{
				TempData["mesaj"] = "Lütfen tüm zorunlu alanları doldurun!";
				ViewBag.Salonlar = _context.Salonlar.ToList(); // Salon listesini tekrar dolduruyoruz
				return View(c);
			}

			// Aynı isimde çalışan var mı kontrolü
			var existingCalisan = _context.Calisanlar
				.FirstOrDefault(x => x.CalisanAd == c.CalisanAd &&
									 x.CalisanSoyad == c.CalisanSoyad &&
									 x.UzmanlikAlani == c.UzmanlikAlani);

			if (existingCalisan != null)
			{
				TempData["mesaj"] = "Bu çalışan zaten mevcut. Lütfen farklı bir çalışan giriniz!";
				ViewBag.Salonlar = _context.Salonlar.ToList(); // Salon listesini tekrar dolduruyoruz
				return View(c);
			}

			// Yeni çalışanı ekliyoruz
			_context.Calisanlar.Add(c);
			_context.SaveChanges();

			TempData["mesaj"] = $"{c.CalisanAd} {c.CalisanSoyad} ({c.UzmanlikAlani}) bilgileri ile çalışan başarıyla eklenmiştir.";
			return RedirectToAction("Index");

		}
		// Details: Show details of a specific employee
		public IActionResult CalisanDetay(int? id)
		{
			if (id == null)
			{
				TempData["hata"] = "Çalışan ID bilgisi eksik.";
				return RedirectToAction("Index");
			}

			var calisan = _context.Calisanlar.Include(c => c.Salon).FirstOrDefault(c => c.CalisanID == id);

			if (calisan == null)
			{
				TempData["hata"] = "Çalışan bulunamadı.";
				return RedirectToAction("Index");
			}

			return View(calisan);
		}

		// Edit: Show the form to edit an employee's details
		public IActionResult CalisanDuzenle(int id)
		{
			// Çalışanı ID'ye göre veritabanından alıyoruz
			var calisan = _context.Calisanlar.FirstOrDefault(c => c.CalisanID == id);

			if (calisan == null)
			{
				return NotFound(); // Çalışan bulunamazsa, 404 döndür
			}

			// Salonlar için ViewBag'e salonları yüklüyoruz
			ViewBag.Salonlar = _context.Salonlar.ToList();

			return View(calisan); // Çalışan verisini formda göstermek için view'e gönderiyoruz
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CalisanDuzenle(Calisan calisan)
		{
			try
			{
				// Çalışan verisini veritabanından alıyoruz
				var mevcutCalisan = _context.Calisanlar.FirstOrDefault(c => c.CalisanID == calisan.CalisanID);

				if (mevcutCalisan == null)
				{
					return NotFound(); // Çalışan bulunamazsa, 404 döndür
				}

				// Çalışanın bilgilerini güncelliyoruz
				mevcutCalisan.CalisanAd = calisan.CalisanAd;
				mevcutCalisan.CalisanSoyad = calisan.CalisanSoyad;
				mevcutCalisan.UzmanlikAlani = calisan.UzmanlikAlani;

				// Model doğrulama işlemi yapıyoruz
				if (!ModelState.IsValid)
				{
					_context.SaveChanges(); // Değişiklikleri kaydediyoruz
					return RedirectToAction("Index"); // Güncelleme başarılı ise, Index sayfasına yönlendir
				}
				else
				{
					// Model geçersizse, hata mesajlarını döndür
					ModelState.AddModelError("", "Geçersiz veriler. Lütfen tekrar deneyin.");
					return View(calisan); // Hatalı formu kullanıcıya geri gönder
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", $"Bir hata oluştu: {ex.Message}");
				return View(calisan); // Hata durumunda formu tekrar gösteriyoruz
			}
		}



		// Delete: Delete an employee
		public IActionResult CalisanSil(int? id)
		{
			if (id == null)
			{
				TempData["hata"] = "Çalışan ID bilgisi eksik.";
				return RedirectToAction("Index");
			}

			var calisan = _context.Calisanlar.FirstOrDefault(c => c.CalisanID == id);
			if (calisan == null)
			{
				TempData["hata"] = "Çalışan bulunamadı.";
				return RedirectToAction("Index");
			}

			_context.Calisanlar.Remove(calisan);
			_context.SaveChanges();
			TempData["mesaj"] = "Çalışan başarıyla silinmiştir.";
			return RedirectToAction("Index");
		}

		public IActionResult CalisanHata()
		{
			return View();
		}

	}
}

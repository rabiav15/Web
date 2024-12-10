using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;

namespace SalonYonetimUygulamasi.Controllers
{
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
		public IActionResult CalisanDuzenle(int? id )
		{
			if (id is null)
			{
				TempData["hata"] = "Düzenleme İçin SalonID Gerekli,Kontrol Edin!";
				return View("CalisanHata");
			}
			var s = _context.Calisanlar.FirstOrDefault(x => x.CalisanID == id);
			if (s == null)
			{
				TempData["mesaj"] = "Lütfen Geçerli Bir ID Girin!";
				return View("CalisanHata");
			}
			return View(s);
		}

		[HttpPost]
		public IActionResult CalisanDuzenle(int id, Calisan c)
		{
			if (id != c.CalisanID)
			{
				TempData["hata"] = "Olmayan Çalışanı Düzenlemeye Çalışıyorsunuz...";
				return View("CalisanHata");
			}
			if (!ModelState.IsValid)
			{
				_context.Calisanlar.Update(c);
				_context.SaveChanges();

				TempData["mesaj"] = c.CalisanAd + "  Adlı Çalışanın Bilgileri Başarıyla Güncellenmiştir!";
				return RedirectToAction("Index");
			}
			TempData["hata"] = "Lütfen Alanları Eksiksiz Doldurunuz!";
			return View("CalisanHata");
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

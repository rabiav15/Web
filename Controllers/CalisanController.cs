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
			if (!ModelState.IsValid)
			{
				// Check if a salon with the same name already exists
				var existingCalisan = _context.Calisanlar
					.FirstOrDefault(x => x.CalisanAd == c.CalisanAd || x.CalisanSoyad == c.CalisanSoyad || x.UzmanlikAlani == c.UzmanlikAlani );

				if (existingCalisan!= null)
				{
					// If a salon with the same name/phone/address exists, show an error message
					TempData["mesaj"] = "Bu Çalışan Zaten Mevcut,Lütfen Farklı Bir Çalışan Giriniz!";
					return View();
				}

				// If no duplicate found, add the new salon
				_context.Calisanlar.Add(c);
				_context.SaveChanges();

				TempData["mesaj"] = c.CalisanAd + " " + c.CalisanSoyad + " " + c.UzmanlikAlani+ " bilgileri bulunan çalışan başarıyla eklenmiştir.";
				return RedirectToAction("Index");
			}

			TempData["mesaj"] = "Ekleme Başarısız!";
			return View();
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
		public IActionResult CalisanDuzenle(int? id)
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

			ViewBag.Salonlar = _context.Salonlar.ToList(); // For dropdown list
			return View(calisan);
		}

		// Edit (POST): Save the changes to an employee's details
		[HttpPost]
		public IActionResult CalisanDuzenle(int id, Calisan calisan)
		{
			if (id != calisan.CalisanID)
			{
				TempData["hata"] = "Geçersiz çalışan ID.";
				return RedirectToAction("Index");
			}

			if (ModelState.IsValid)
			{
				_context.Calisanlar.Update(calisan);
				_context.SaveChanges();
				TempData["mesaj"] = "Çalışan başarıyla güncellenmiştir.";
				return RedirectToAction("Index");
			}

			ViewBag.Salonlar = _context.Salonlar.ToList(); // For dropdown list
			TempData["hata"] = "Çalışan güncellenemedi. Lütfen tekrar deneyin.";
			return View(calisan);
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
	}
}

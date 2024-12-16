
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;


namespace SalonYonetimUygulamasi.Controllers
{
	
	public class SalonController : Controller
	{
		private readonly SalonContext _context;

		public SalonController(SalonContext context)
		{
			_context = context;
		}


		public IActionResult Index()
		{

			var s = _context.Salonlar.ToList();
			return View(s);
		}
		public IActionResult SalonEkle()
		{

			return View();

		}
		[HttpPost]
		public IActionResult SalonEkle(Salon s)
		{
			if (!ModelState.IsValid)
			{
				// Check if a salon with the same name already exists
				var existingSalon = _context.Salonlar
					.FirstOrDefault(x => x.SalonAd == s.SalonAd || x.SalonTelefon == s.SalonTelefon || x.SalonAdres == s.SalonAdres);

				if (existingSalon != null)
				{
					// If a salon with the same name/phone/address exists, show an error message
					TempData["mesaj"] = "Bu Salon Zaten Mevcut,Lütfen Farklı Bir Salon Giriniz!";
					return View();
				}

				// If no duplicate found, add the new salon
				_context.Salonlar.Add(s);
				_context.SaveChanges();

				TempData["mesaj"] = s.SalonAd + " " + s.SalonAdres + " " + s.SalonTelefon + " bilgileri bulunan salon başarıyla eklenmiştir.";
				return RedirectToAction("Index");
			}

			TempData["mesaj"] = "Ekleme Başarısız!";
			return View();
		}
		


		public IActionResult SalonDetay(int? id)
		{
			if(id is null)
			{
				TempData["hata"] = "Lütfen Salon Id Bilgisini Giriniz";
				return View("SalonHata");
			}

			var s =_context.Salonlar.Include(x=> x.Calisanlar).FirstOrDefault(x=> x.SalonID == id); 
			 
			
				if (s == null) { 
					TempData["mesaj"] = "Lülfen Geçerli Bir ID Girin!";
				return View("SalonHata");

			}
			  return View(s);
		}

		public IActionResult SalonDuzenle(int? id)
		{
			if (id is null)
			{
				TempData["hata"] = "Düzenleme İçin SalonID Gerekli,Kontrol Edin!";
				return View("SalonHata");
			}
			var s = _context.Salonlar.FirstOrDefault(x => x.SalonID == id);
			if (s == null)
			{
				TempData["mesaj"] = "Lütfen Geçerli Bir ID Girin!";
				return View("SalonHata");
			}
			return View(s);
		}

		[HttpPost]
		public IActionResult SalonDuzenle(int? id, Salon s)
		{
			if (id != s.SalonID)
			{
				TempData["hata"] = "Olmayan Salonu Düzenlemeye Çalışıyorsunuz...";
				return View("SalonHata");
			}
			if (!ModelState.IsValid)
			{
				_context.Salonlar.Update(s);
				_context.SaveChanges();

				TempData["mesaj"] = s.SalonAd + "  Adlı Salonun Bilgileri Başarıyla Güncellenmiştir!";
				return RedirectToAction("Index");
			}
			TempData["hata"] = "Lütfen Alanları Eksiksiz Doldurunuz!";
			return View("SalonHata");
		}

        public IActionResult SalonSil(int? id)
        {
            if (id is null)
            {
                TempData["hata"] = "Silme için id gerekli,lütfen id giriniz";
                return View("SalonHata");
            }
            var s = _context.Salonlar.Include(x => x.Calisanlar).FirstOrDefault(x => x.SalonID == id);


            if (s == null)
            {
                TempData["hata"] = "  Salon Sistemde Bulunmamaktır,Kontrol Edin!";
                return View("SalonHata");
            }
            if (s.Calisanlar.Count > 0)
            {
                TempData["hata"] = "  Salonuna Ait Çalışanlar Var,Kontrol Edin!";
                return View("SalonHata");

            }

            _context.Salonlar.Remove(s);
            _context.SaveChanges();
            TempData["mesaj"] = s.SalonAd + "  Adlı Salon Sistemden Silinmiştir!";
            return RedirectToAction("Index");
        }

    }
}

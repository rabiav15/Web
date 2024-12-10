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
		public IActionResult RandevuCreate(Randevu randevu)
		{
			randevu.Tarih = DateTime.Parse($"{randevu.Tarih.ToString("yyyy-MM-dd")} {randevu.Tarih.TimeOfDay}");

			// Fiyat atama
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

			if (!ModelState.IsValid)
			{
				_context.Randevular.Add(randevu);
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			// Hatalı giriş durumunda seçenekleri yeniden gönderiyoruz.
			ViewBag.Dates = new List<string>();
			var now = DateTime.Now.Date;
			for (int i = 0; i < 7; i++)
			{
				ViewBag.Dates.Add(now.AddDays(i).ToString("yyyy-MM-dd"));
			}
			ViewBag.Hours = new List<string>
			{
				"09:00", "09:30", "10:00", "10:30", "11:00", "11:30",
				"12:00", "13:30", "14:00", "14:30", "15:00", "15:30",
				"16:00", "16:30"
			};

			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			ViewBag.Islemler = new[] { "Makyaj", "Saç" };
			return View(randevu);



		}

	}

	}


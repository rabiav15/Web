using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonYonetimUygulamasi.Models;
namespace SalonYonetimUygulamasi.Controllers
{
	public class IslemController : Controller
	{

		private readonly SalonContext _context;

		public IslemController(SalonContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			var islemler = _context.Islemler
	.Include(i => i.Calisan)
	.ToList();
			return View(islemler);

			
		}

		public IActionResult IslemCreate()
		{
			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			return View();
		}

		// POST: Islem/IslemCreate (Yeni işlem ekleme işlemi)
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult IslemCreate(Islem islem)
		{
			if (!ModelState.IsValid)
			{
				_context.Islemler.Add(islem);
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			return View(islem);
		}

		public IActionResult IslemDetay(int id)
		{
			var islem = _context.Islemler
				.Include(i => i.Calisan)
				.FirstOrDefault(i => i.IslemID == id);

			if (islem == null)
			{
				return NotFound();
			}

			return View(islem);
		}

		public IActionResult IslemDuzenle(int id)
		{
			var islem = _context.Islemler.FirstOrDefault(i => i.IslemID == id);

			if (islem == null)
			{
				return NotFound();
			}

			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			return View(islem);
		}

		// POST: Islem/IslemDuzenle/5 (Düzenleme işlemi)
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult IslemDuzenle(Islem islem)
		{
			if (!ModelState.IsValid)
			{
				_context.Update(islem);
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Calisanlar = _context.Calisanlar.ToList();
			return View(islem);
		}
		public IActionResult IslemSil(int id)
		{
			var islem = _context.Islemler
				.Include(i => i.Calisan)
				.FirstOrDefault(i => i.IslemID == id);

			if (islem == null)
			{
				return NotFound();
			}

			return View(islem);
		}

		// POST: Islem/IslemSil/5 (Silme işlemi)
		[HttpPost, ActionName("IslemSil")]
		[ValidateAntiForgeryToken]
		public IActionResult IslemSilConfirmed(int id)
		{
			var islem = _context.Islemler.Find(id);

			if (islem != null)
			{
				_context.Islemler.Remove(islem);
				_context.SaveChanges();
			}

			return RedirectToAction(nameof(Index));
		
	}
}
}

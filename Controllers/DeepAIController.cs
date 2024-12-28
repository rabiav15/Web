using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace SalonYonetimUygulamasi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeepAIController : ControllerBase
    {
        private readonly DeepAIService _deepAiService;

        public DeepAIController()
        {
            _deepAiService = new DeepAIService();  // DeepAIService'i enjekte ediyoruz
        }

        // Fotoğraf Yükleme İşlemi ve API'den Saç Rengi Önerisi Alınması
        [HttpPost("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Lütfen bir dosya yükleyin.");
            }

            // Fotoğrafı server'a kaydediyoruz
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);

            // Dosyayı kaydediyoruz
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // DeepAI API ile saç rengi önerileri alıyoruz
            var aiResponse = await _deepAiService.GetHairColorSuggestionsAsync(filePath);

            // API'den dönen yanıtı JSON olarak döndürüyoruz
            if (aiResponse == "Saç rengi önerisi alınamadı.")
            {
                return NotFound(aiResponse);  // Eğer öneri alınamadıysa 404 döndürüyoruz
            }

            // Başarıyla yanıt alındığında, JSON formatında döndürürüz
            return Ok(new { outputUrl = aiResponse });
        }

        // Index View için (Web API'de View kullanılmaz)
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok("API çalışıyor!");
        }
    }
}

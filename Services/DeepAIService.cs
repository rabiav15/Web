using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using SalonYonetimUygulamasi.Models;


public class DeepAIService
{
	private readonly string _apiKey = "bf13b87a-72e0-493f-a945-28029369d43b";
	private readonly string _apiUrl = "https://api.deepai.org/api/text2img \r\n";

	public async Task<string> GetHairColorSuggestionsAsync(string imagePath)
	{
		try
		{
			using (var client = new HttpClient())
			{
				// API Anahtarını header'a ekliyoruz
				client.DefaultRequestHeaders.Add("Api-Key", _apiKey);

				// Fotoğrafı okuma ve API'ye gönderme
				using (var formData = new MultipartFormDataContent())
				{
					var imageContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
					formData.Add(imageContent, "image", Path.GetFileName(imagePath));

					// API'ye POST isteği gönderme
					var response = await client.PostAsync(_apiUrl, formData);
					var responseString = await response.Content.ReadAsStringAsync();

					// JSON yanıtını işleyelim
					var jsonResponse = JObject.Parse(responseString);
					var outputUrl = jsonResponse["output_url"]?.ToString();  // Örnek: response'dan output_url'yi al

					return outputUrl ?? "Saç rengi önerisi alınamadı.";  // URL döndürülür, aksi takdirde hata mesajı
				}
			}
		}
		catch (Exception ex)
		{
			return $"Bir hata oluştu: {ex.Message}";
		}
	}
}


using System.ComponentModel.DataAnnotations;

namespace SalonYonetimUygulamasi.Models
{
	public class LoginView
	{

		[Required(ErrorMessage = "Email adresi gereklidir.")]
		[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre gereklidir.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}

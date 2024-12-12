namespace SalonYonetimUygulamasi.Models
{
	using System.ComponentModel.DataAnnotations;

	

	public class RegisterView
	{
		[Required(ErrorMessage = "Email alanı zorunludur.")]
		[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre alanı zorunludur.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Şifre onayı zorunludur.")]
		[Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}

}

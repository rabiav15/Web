using System.ComponentModel.DataAnnotations;

public class AdminLoginView
{
	[Required(ErrorMessage = "Email gereklidir.")]
	[EmailAddress]
	public string Email { get; set; }

	[Required(ErrorMessage = "Şifre gereklidir.")]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	public bool RememberMe { get; set; }
}


﻿namespace SalonYonetimUygulamasi.Models
{
	using System.ComponentModel.DataAnnotations;

	public class AdminLoginView
	{
		[Required(ErrorMessage = "Email adresi zorunludur.")]
		[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre zorunludur.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}

}

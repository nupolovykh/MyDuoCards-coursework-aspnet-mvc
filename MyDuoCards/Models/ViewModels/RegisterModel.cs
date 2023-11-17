using System.ComponentModel.DataAnnotations;

namespace MyDuoCards.Models.ViewModels
{
	public class RegisterModel
	{
		[Required(ErrorMessage = "Incorrect login given")]
		public string Login { get; set; }

		[Required(ErrorMessage = "Incorrect email given")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Incorrect password given")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Passwords are not the same")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
	}
}

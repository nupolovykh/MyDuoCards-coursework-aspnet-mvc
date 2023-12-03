using MyDuoCards.Models.DBModels;
using System.ComponentModel.DataAnnotations;

namespace MyDuoCards.Models.ViewModels
{
    public class EditUserModel
    {
        public string Login { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public int RoleId { get; set; }

        [Required(ErrorMessage = "Passwords are not the same")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Passwords are not the same")]
        //[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}

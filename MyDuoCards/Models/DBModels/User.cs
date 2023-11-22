using System.Reflection.Metadata;

namespace MyDuoCards.Models.DBModels
{
    public class User
    {
		public int UserId { get; set; }

		public int? RoleId { get; set; }
		public Role? Role { get; set; } = null!;

		public UserDictionary? UserDictionary { get; set; } = null!;

		public string? UserLogin { get; set; }
		public string? UserEmail { get; set; }
		public string? UserPassword { get; set; }

	}
}

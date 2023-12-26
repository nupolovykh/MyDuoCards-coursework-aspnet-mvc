using System.Reflection.Metadata;

namespace MyDuoCards.Models.DBModels
{
    public class User
    {
		public int Id { get; set; }
        public string Login { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
		public Role? Role { get; set; }

        public List<Dictionary>? Dictionaries { get; set; }

		public ICollection<Attendance>? Attendances { get; set; }
	}
}

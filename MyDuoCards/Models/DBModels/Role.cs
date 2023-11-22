using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDuoCards.Models.DBModels
{
	public class Role
	{
		[Key]
		public int RoleId { get; set; }
		public ICollection<User> Users { get; } = new List<User>();

		public string? RoleName { get; set; }
	}
}

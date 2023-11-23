using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDuoCards.Models.DBModels
{
	public class Role
	{
		[Key]
		public int RoleId { get; set; }
		public string? RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

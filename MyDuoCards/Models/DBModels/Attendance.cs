using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace MyDuoCards.Models.DBModels
{
	public class Attendance
	{
		[Key]
		public int Id { get; set; }

		[Column(TypeName = "datetime2")]
		public DateTime? Time { get; set; }

		public int UserId { get; set; }
		public User? User { get; set; }
	}
}

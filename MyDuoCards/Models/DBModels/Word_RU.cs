using System.ComponentModel.DataAnnotations;

namespace MyDuoCards.Models.DBModels
{
	public class RuWord
	{
		[Key]
		public int RuWordId { get; set; }

		public int EuWordId { get; set; }
		public EuWord EuWord { get; set; } = null!;

		public string? RuWriting { get; set; }
	}
}

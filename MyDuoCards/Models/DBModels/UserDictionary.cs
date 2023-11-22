using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace MyDuoCards.Models.DBModels
{
	public class UserDictionary
	{
		[Key]
		public int UserDictionaryId { get; set; }
		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public int? EnWordId { get; set; }
		public EuWord? EuWord { get; set; } = null!;
	}
}

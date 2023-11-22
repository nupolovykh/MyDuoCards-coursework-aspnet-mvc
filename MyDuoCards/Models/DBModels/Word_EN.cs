using System.ComponentModel.DataAnnotations;

namespace MyDuoCards.Models.DBModels
{
	public class EuWord
	{
		[Key]
		public int EnWordId { get; set; }
		public ICollection<UserDictionary> UserDictionaries { get; } = new List<UserDictionary>();

		public string? EnWriting { get; set; }
	}
}

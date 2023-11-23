using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace MyDuoCards.Models.DBModels
{
    //[PrimaryKeys(UserId,EnWordId)]
    public class Dictionary
	{
		[Key]
		public int UserId { get; set; }
		public User User { get; set; }

		public int EnWordId { get; set; }
		public EnWord? EuWord { get; set; }
	}
}

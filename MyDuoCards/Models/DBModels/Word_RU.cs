using System.ComponentModel.DataAnnotations;

namespace MyDuoCards.Models.DBModels
{
	public class RuWord
	{
		[Key]
		public int Id { get; set; }
        public string RuWriting { get; set; }

        public int EnWordId { get; set; }
		public EnWord EnWord { get; set; }

	}
}

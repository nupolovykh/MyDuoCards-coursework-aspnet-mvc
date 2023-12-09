using System.ComponentModel.DataAnnotations;

namespace MyDuoCards.Models.DBModels
{
	public class EnWord
	{
		[Key]
		public int Id { get; set; }
        public string EnWriting { get; set; }

        public List<Dictionary>? Dictionaries { get; set; }

        public RuWord? RuWord { get; set; }

    }
}

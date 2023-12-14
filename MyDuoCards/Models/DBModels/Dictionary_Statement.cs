using System.ComponentModel.DataAnnotations;

namespace MyDuoCards.Models.DBModels
{
    public class DictionaryStatement
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Dictionary>? Dictionaries { get; set; }
    }
}

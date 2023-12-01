using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace MyDuoCards.Models.DBModels
{
    [PrimaryKey(nameof(UserId), nameof(EnWordId))]
    public class Dictionary
	{
        public string? Category { get; set; }
        //[Key, Column(Order = 0)]
        public int UserId { get; set; }
		public User? User { get; set; }

        //[Key, Column(Order = 1)]
        public int EnWordId { get; set; }
		public EnWord? EuWord { get; set; }
	}
}

using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models.DBModels;

namespace MyDuoCards.Models
{
    public class MsApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public MsApplicationContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}

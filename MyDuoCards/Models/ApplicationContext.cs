using Microsoft.EntityFrameworkCore;
using MyDuoCards.Models.DBModels;
using MyDuoCards.Models.Extensions;
using System.Configuration;

namespace MyDuoCards.Models

{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
		public DbSet<Role> Roles { get; set; } = null!;
		public DbSet<Dictionary> Dictionaries { get; set; } = null!;
		public DbSet<EnWord> EuWords { get; set; } = null!;
		public DbSet<RuWord> RuWords { get; set; } = null!;


		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            if (Database.EnsureCreated())
            {
				var roles = Set<Role>();
				roles.Add(new Role { RoleName = "Admin" });
				roles.Add(new Role { RoleName = "User" });

				var venus = Set<User>();
				venus.Add(new User { UserLogin = "Minako", UserEmail = "venus@su", UserPassword = "beam".ToHash(), RoleId = 1});

				SaveChanges();
			}
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = CreatingPath();
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }

        private string CreatingPath()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string parentDirectory = Directory.GetParent(currentDirectory)!.FullName;
            return Path.Combine(parentDirectory, "SQLiteDatabaseBrowserPortable", "Database", "fiction.db");
        }
    }
}

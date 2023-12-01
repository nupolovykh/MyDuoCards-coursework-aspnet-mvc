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
		public DbSet<EnWord> EnWords { get; set; } = null!;
		public DbSet<RuWord> RuWords { get; set; } = null!;
        public DbSet<Attandance> Attandances { get; set; } = null!;


		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            if (Database.EnsureCreated())
            {
				var roles = Set<Role>();
				roles.Add(new Role { Name = "Admin" });
				roles.Add(new Role { Name = "User" });

                var venus = Set<User>();
				venus.Add(new User { Login = "Minako", Email = "venus@su", Password = "beam".ToHash(), RoleId = 1});

                var enWords = Set<EnWord>();
                enWords.Add(new EnWord { EnWriting = "something" });

                var ruWords = Set<RuWord>();
                ruWords.Add(new RuWord { EnWordId = 1, RuWriting = "Что-то"});

				//SaveChanges();
				//var attandances = Set<Attandance>();
				//attandances.Add(new Attandance { UserId = 1,  Time = DateTime.Now});
				//await SaveChangesAsync();

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

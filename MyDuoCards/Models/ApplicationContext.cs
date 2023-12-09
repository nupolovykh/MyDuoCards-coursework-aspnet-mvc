using Bogus;
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
			Database.EnsureDeleted();
			Database.EnsureCreated();

            //if (Database.EnsureCreated())
            //{
            //    var roles = Set<Role>();
            //    roles.Add(new Role { Id = 1, Name = "Admin" });
            //    roles.Add(new Role { Id = 2, Name = "User" });

            //    var venus = Set<User>();
            //    venus.Add(new User { Id = 1, Login = "Minako", Email = "venus@su", Password = "beam".ToHash(), RoleId = 1 });

            //    var enWords = Set<EnWord>();
            //    enWords.Add(new EnWord { Id = 1, EnWriting = "something" });

            //    var ruWords = Set<RuWord>();
            //    ruWords.Add(new RuWord { Id = 1, EnWordId = 1, RuWriting = "Что-то" });

            //    var attandances = Set<Attandance>();
            //    attandances.Add(new Attandance { Id = 1, UserId = 1, Time = DateTime.Now });

            //    SaveChanges();
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Item>()
            //.ToTable("Item")
            //.HasKey(p => p.Id);

            modelBuilder.Entity<Role>()
                .HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" });

            modelBuilder.Entity<User>()
                .HasData(new User { Id = 1, Login = "Minako", Email = "venus@su", Password = "beam".ToHash(), RoleId = 1 });

            modelBuilder.Entity<Attandance>()
                .HasData(new Attandance { Id = 1, UserId = 1, Time = DateTime.Now });

			var fakerEn = new Faker("en");
			var fakerRu = new Faker("ru");

            List<EnWord> enWords = new List<EnWord>();
            for (int i = 1; i < 1000; i++) 
            {
                enWords.Add(new EnWord { Id = i, EnWriting = fakerEn.Lorem.Word() });
            }


			List<RuWord> ruWords = new List<RuWord>();
			for (int i = 1; i < 1000; i++)
			{
				ruWords.Add(new RuWord { Id = i, RuWriting = fakerRu.Lorem.Word(), EnWordId = i });
			}

			modelBuilder.Entity<EnWord>().HasData(enWords);
			modelBuilder.Entity<RuWord>().HasData(ruWords);

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

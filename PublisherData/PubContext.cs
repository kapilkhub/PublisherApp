using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublisherDomain;
using System.ComponentModel;

namespace PublisherData
{
	public class PubContext : DbContext
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public PubContext()
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PubDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;")
			   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
			   .LogTo(Console.WriteLine,
					new[] { DbLoggerCategory.Database.Command.Name },
					LogLevel.Information
					)
			   .EnableSensitiveDataLogging(true);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Author>().HasMany<Book>().WithOne();
		}
	}
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublisherData.Migrations;
using PublisherDomain;

namespace PublisherData
{
	public class PubContext : DbContext
	{
		public DbSet<Book> Books { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Artist> Artists { get; set; }
		public DbSet<Cover> Covers { get; set; }

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

			modelBuilder.Entity<Artist>().HasMany(a => a.Covers).WithMany(c => c.Artists)
				.UsingEntity<CoverAssignment>(ca =>
				{
					ca.Property(b => b.ArtistId).HasColumnName("ArtistsArtistId");
					ca.Property(b => b.CoverId).HasColumnName("CoversCoverId");
					ca.Property(b => b.DateCreated).HasDefaultValueSql("CURRENT_TIMESTAMP");
					ca.ToTable("ArtistCover");
				});

		}
	}
}

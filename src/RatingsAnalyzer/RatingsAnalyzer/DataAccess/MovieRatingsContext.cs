using System.Configuration;
using Microsoft.EntityFrameworkCore;
using RatingsAnalyzer.Model;

namespace RatingsAnalyzer.DataAccess
{
    public class MovieRatingsContext: DbContext
    {
        public DbSet<MovieData> Movies { get; set; } 
        public DbSet<MovieRating> MovieRatings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieData>().HasAlternateKey(md => md.Title);
            
            modelBuilder.Entity<MovieRating>().HasIndex(mr => mr.Uri);
            modelBuilder.Entity<MovieRating>().HasIndex(mr => mr.Source);
        }
    }
}

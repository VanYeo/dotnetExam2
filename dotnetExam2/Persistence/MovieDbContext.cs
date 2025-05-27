using dotnetExam2.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetExam2.Persistence
{
    // allows configuration of Movie entities and DB context (provider, connection string, etc.)
    public class MovieDbContext(DbContextOptions<MovieDbContext> options) : DbContext(options)
    {
        // maps directly to the "Movies" table in the database
        public DbSet<Movie> Movies => Set<Movie>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // sets schema to "app" for all tables in this context
            modelBuilder.HasDefaultSchema("app");

            // ensures all entities in the assembly are configured automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieDbContext).Assembly);

            // ensures default configurations defined by DbContext are applied
            base.OnModelCreating(modelBuilder);
        }
    }
}

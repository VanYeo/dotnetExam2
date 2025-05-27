using dotnetExam2.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnetExam2.Persistence
{
    // allows configuration of Movie entities and DB context (provider, connection string, etc.)
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

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

        // check if movie existst in DB and if not, adds a new movie
        // automatically updates the database schema to match the model
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseAsyncSeeding(async (context, _, cancellationToken) =>
                {
                    var sampleMovie = await context.Set<Movie>().FirstOrDefaultAsync(b => b.Title == "Sonic the Hedgehog 3");
                    if (sampleMovie == null)
                    {
                        sampleMovie = Movie.Create("Sonic the Hedgehog 3", "Fantasy", new DateTimeOffset(new DateTime(2025, 1, 3), TimeSpan.Zero), 7);
                        await context.Set<Movie>().AddAsync(sampleMovie);
                        await context.SaveChangesAsync();
                    }
                })
                .UseSeeding((context, _) =>
                {
                    var sampleMovie = context.Set<Movie>().FirstOrDefault(b => b.Title == "Sonic the Hedgehog 3");
                    if (sampleMovie == null)
                    {
                        sampleMovie = Movie.Create("Sonic the Hedgehog 3", "Fantasy", new DateTimeOffset(new DateTime(2025, 1, 3), TimeSpan.Zero), 7);
                        context.Set<Movie>().Add(sampleMovie);
                        context.SaveChanges();
                    }
                });
        }
    }
}

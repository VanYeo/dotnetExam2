using dotnetExam2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace dotnetExam2.Persistence.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        // define DB mapping details eg table names, keys, relationships, constraints, etc.
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            // Define table name
            builder.ToTable("Movies");

            // Set primary key
            builder.HasKey(m => m.Id);

            // Configure properties

            // ensure title property is required and has max length   
            builder.Property(m => m.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.Genre)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.ReleaseDate)
                   .IsRequired();

            builder.Property(m => m.Rating)
                   .IsRequired();

            // Configure Created and LastModified properties to be handled as immutable and modifiable timestamps

            // ensures Created is set on insert
            builder.Property(m => m.Created)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            // LastModified is updated on each update
            builder.Property(m => m.LastModified)
                   .IsRequired()
                   .ValueGeneratedOnUpdate();

            // Optional: Add indexes for better query performance
            builder.HasIndex(m => m.Title);
        }
    }
}

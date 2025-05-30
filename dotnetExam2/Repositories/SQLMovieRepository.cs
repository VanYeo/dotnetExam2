using dotnetExam2.Models;
using dotnetExam2.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace dotnetExam2.Repositories
{
    public class SQLMovieRepository : IMovieRepository
    {
        private readonly MovieDbContext dbContext;
        public SQLMovieRepository(MovieDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Movie> CreateAsync(Movie movie)
        {
            await dbContext.Movies.AddAsync(movie);
            await dbContext.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie>? DeleteAsync(Guid id)
        {
            var existingMovie = await dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);

            if (existingMovie == null)
            {
                return null;
            }

            dbContext.Movies.Remove(existingMovie);
            await dbContext.SaveChangesAsync();
            return existingMovie;
        }

        public async Task<List<Movie>> GetAllMoviesAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            // becomes a variable of type IQueryable<Movie> to allow for further filtering or sorting
            var movies = dbContext.Movies.AsQueryable();

            // filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                movies = filterOn.ToLower() switch
                {
                    "title" => movies.Where(movie => movie.Title.ToLower().Contains(filterQuery.ToLower())),
                    "genre" => movies.Where(movie => movie.Genre.ToLower().Contains(filterQuery.ToLower())),
                    "releasedate" when DateTime.TryParse(filterQuery, out var date) =>
                        movies.Where(movie => movie.ReleaseDate.Date == date.Date),
                    "rating" when double.TryParse(filterQuery, out var rating) =>
                        movies.Where(movie => movie.Rating == rating),
                    _ => movies
                };
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                bool asc = isAscending ?? true;
                movies = (sortBy.ToLower(), asc) switch
                {
                    ("title", true) => movies.OrderBy(movie => movie.Title),
                    ("title", false) => movies.OrderByDescending(movie => movie.Title),
                    ("rating", true) => movies.OrderBy(movie => movie.Rating),
                    ("rating", false) => movies.OrderByDescending(movie => movie.Rating),
                    ("releasedate", true) => movies.OrderBy(movie => movie.ReleaseDate),
                    ("releasedate", false) => movies.OrderByDescending(movie => movie.ReleaseDate),
                    _ => movies
                };
            }

            // pagination
            var skipResults = (pageNumber - 1) * pageSize;


            return await movies.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(Guid id)
        {
            return await dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);
        }

        public async Task<Movie?> UpdateAsync(Guid id, Movie movie)
        {
            var existingMovie = await dbContext.Movies.FirstOrDefaultAsync(movie => movie.Id == id);

            existingMovie.Update(
                movie.Title,
                movie.Genre,
                movie.ReleaseDate,
                movie.Rating
            );

            await dbContext.SaveChangesAsync();
            return existingMovie;

        }
    }
}

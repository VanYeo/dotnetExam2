using dotnetExam2.DTOs;
using dotnetExam2.Models;
using dotnetExam2.Persistence;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace dotnetExam2.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDbContext _dbContext;
        private readonly ILogger<MovieService> _logger;

        public MovieService(MovieDbContext dbContext, ILogger<MovieService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // get all movies 
        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            return await _dbContext.Movies
                .AsNoTracking()
                .Select(movie => new MovieDto(
                    movie.Id,
                    movie.Title,
                    movie.Genre,
                    movie.ReleaseDate,
                    movie.Rating
                ))
                .ToListAsync();
        }

        // get movie by id
        public async Task<MovieDto?> GetMovieByIdAsync(Guid id)
        {
            // access movies table in db via DbContext 
            var movie = await _dbContext.Movies
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return null;

            return new MovieDto(
                movie.Id,
                movie.Title,
                movie.Genre,
                movie.ReleaseDate,
                movie.Rating
            );
        }

        // create movie and return movie
        public async Task<MovieDto> CreateMovieAsync(CreateMovieDto command)
        {
            var movie = Movie.Create
                (command.Title, command.Genre, command.ReleaseDate, command.Rating);

            await _dbContext.Movies.AddAsync(movie);
            await _dbContext.SaveChangesAsync();

            return new MovieDto(
               movie.Id,
               movie.Title,
               movie.Genre,
               movie.ReleaseDate,
               movie.Rating
            );
        }

        // update movie by id
        public async Task UpdateMovieAsync(Guid id, UpdateMovieDto command)
        {
            var movieToUpdate = await _dbContext.Movies.FindAsync(id);
            if (movieToUpdate is null)
                throw new ArgumentNullException($"Invalid Movie Id.");
            movieToUpdate.Update(command.Title, command.Genre, command.ReleaseDate, command.Rating);
            await _dbContext.SaveChangesAsync();
        }

        // delete movie by id
        public async Task DeleteMovieAsync(Guid id)
        {
            var movieToDelete = await _dbContext.Movies.FindAsync(id);
            if (movieToDelete != null)
            {
                _dbContext.Movies.Remove(movieToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MovieDto>> SearchMoviesByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Enumerable.Empty<MovieDto>();

            var movies = await _dbContext.Movies
                .AsNoTracking()
                .Where(m => EF.Functions.Like(m.Title.ToLower(), $"%{title.ToLower()}%"))
                .Select(movie => new MovieDto(
                    movie.Id,
                    movie.Title,
                    movie.Genre,
                    movie.ReleaseDate,
                    movie.Rating
                ))
                .ToListAsync();

            return movies;
        }

        //public async Task<IEnumerable<MovieDto>> GetMoviesSortedByRatingDescAsync()
        //{
        //    var movies = await _dbContext.Movies
        //        .AsNoTracking()
        //        .OrderByDescending(m => m.Rating)
        //        .Select(movie => new MovieDto(
        //            movie.Id,
        //            movie.Title,
        //            movie.Genre,
        //            movie.ReleaseDate,
        //            movie.Rating
        //        ))
        //        .ToListAsync();

        //    return movies;
        //}

        //public async Task<IEnumerable<MovieDto>> GetMoviesSortedByRatingAscAsync()
        //{
        //    var movies = await _dbContext.Movies
        //        .AsNoTracking()
        //        .OrderBy(m => m.Rating)
        //        .Select(movie => new MovieDto(
        //            movie.Id,
        //            movie.Title,
        //            movie.Genre,
        //            movie.ReleaseDate,
        //            movie.Rating
        //        ))
        //        .ToListAsync();

        //    return movies;

        // Helper method to reduce duplication for sorting by rating
        private async Task<IEnumerable<MovieDto>> GetMoviesSortedByRatingAsync(bool descending)
        {
            var query = _dbContext.Movies.AsNoTracking();
            query = descending ? query.OrderByDescending(m => m.Rating) : query.OrderBy(m => m.Rating);

            return await query
                .Select(movie => new MovieDto(
                    movie.Id,
                    movie.Title,
                    movie.Genre,
                    movie.ReleaseDate,
                    movie.Rating
                ))
                .ToListAsync();
        }

        public Task<IEnumerable<MovieDto>> GetMoviesSortedByRatingDescAsync()
        {
            return GetMoviesSortedByRatingAsync(descending: true);
        }

        public Task<IEnumerable<MovieDto>> GetMoviesSortedByRatingAscAsync()
        {
            return GetMoviesSortedByRatingAsync(descending: false);
        }
    }
}

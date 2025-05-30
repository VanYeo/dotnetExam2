using dotnetExam2.Models;
using dotnetExam2.Repositories;

namespace dotnetExam2.Services
{
    public class MovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<Movie>> GetAllMoviesAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            return await _movieRepository.GetAllMoviesAsync(
                filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
        }

        public async Task<Movie?> GetByIdAsync(Guid id)
        {
            return await _movieRepository.GetByIdAsync(id);
        }

        public async Task<Movie> CreateAsync(string title, string genre, DateTimeOffset releaseDate, double rating)
        {
            if (rating < 0.0 || rating > 10.0)
                throw new ArgumentException("Rating must be between 0.0 and 10.0.");

            if (releaseDate > DateTimeOffset.UtcNow)
                throw new ArgumentException("Release date cannot be in the future.");

            var movie = Movie.Create(title, genre, releaseDate, rating);
            return await _movieRepository.CreateAsync(movie);
        }

        public async Task<Movie?> UpdateAsync(Guid id, Movie updatedMovie)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
            {
                return null;
            }
            if (updatedMovie.Rating < 0.0 || updatedMovie.Rating > 10.0)
                throw new ArgumentException("Rating must be between 0.0 and 10.0.");

            if (updatedMovie.ReleaseDate > DateTimeOffset.UtcNow)
                throw new ArgumentException("Release date cannot be in the future.");

            return await _movieRepository.UpdateAsync(id, updatedMovie);
        }

        public async Task<Movie?> DeleteAsync(Guid id)
        {
            return await _movieRepository.DeleteAsync(id);
        }
    }
}
using dotnetExam2.DTOs;

namespace dotnetExam2.Services
{
    public interface IMovieService
    {
        // returns MovieDto if created successfully after async operation
        Task<MovieDto> CreateMovieAsync(CreateMovieDto command);

        // get movie by Id and returns MovieDto or null
        Task<MovieDto?> GetMovieByIdAsync(Guid id);
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync();
        // takes an Id and UpdateMovieDto to update the movie
        Task UpdateMovieAsync(Guid id, UpdateMovieDto command);
        Task DeleteMovieAsync(Guid id);
        Task<IEnumerable<MovieDto>> SearchMoviesByTitleAsync(string title);
        
        /// Returns all movies sorted from highest to lowest rating.
        Task<IEnumerable<MovieDto>> GetMoviesSortedByRatingDescAsync();
        Task<IEnumerable<MovieDto>> GetMoviesSortedByRatingAscAsync();
    }
}

using dotnetExam2.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace dotnetExam2.Repositories
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllMoviesAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true,
            int pageNumber = 1, int pageSize = 1000);

        Task<Movie?> GetByIdAsync(Guid id);

        Task<Movie> CreateAsync(Movie movie);

        Task<Movie?> UpdateAsync(Guid id, Movie movie);

        Task<Movie>? DeleteAsync(Guid id);
    }
}

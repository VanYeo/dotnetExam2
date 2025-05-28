using dotnetExam2.DTOs;
using dotnetExam2.Services;

namespace dotnetExam2.Endpoints
{
    public static class MovieEndpoints
    {
        public static void MapMovieEndpoints(this IEndpointRouteBuilder routes)
        {
            // creates new group of endpoints under base path "/api/movies" and tags as "movies"
            var movieApi = routes.MapGroup("/api/movies").WithTags("Movies");

            movieApi.MapPost("/", async (IMovieService service, CreateMovieDto command) =>
            {
                var movie = await service.CreateMovieAsync(command);
                return TypedResults.Created($"/api/movies/{movie.Id}", movie);
            });

            movieApi.MapGet("/", async (IMovieService service) =>
            {
                var movies = await service.GetAllMoviesAsync();
                return TypedResults.Ok(movies);
            });

            movieApi.MapGet("/search", async (IMovieService service, string title) =>
            {
                var movies = await service.SearchMoviesByTitleAsync(title);
                return TypedResults.Ok(movies);
            });

            movieApi.MapGet("/{id:guid}", async (IMovieService service, Guid id) =>
            {
                var movie = await service.GetMovieByIdAsync(id);

                return movie is null
                    ? (IResult)TypedResults.NotFound(new { Message = $"Movie with ID {id} not found." })
                    : TypedResults.Ok(movie);
            });

            movieApi.MapGet("/sort/highest-rating", async (IMovieService service) =>
            {
                var movies = await service.GetMoviesSortedByRatingDescAsync();

                return TypedResults.Ok(movies);

            });

            movieApi.MapGet("/sort/lowest-rating", async (IMovieService service) =>
            {
                var movies = await service.GetMoviesSortedByRatingAscAsync();

                return TypedResults.Ok(movies);

            });

            movieApi.MapPut("/{id}", async (IMovieService service, Guid id, UpdateMovieDto command) =>
            {
                await service.UpdateMovieAsync(id, command);
                return TypedResults.NoContent();
            });

            movieApi.MapDelete("/{id}", async (IMovieService service, Guid id) =>
            {
                await service.DeleteMovieAsync(id);
                return TypedResults.NoContent();
            });
        }
    }
}

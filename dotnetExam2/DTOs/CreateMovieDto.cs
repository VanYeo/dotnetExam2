namespace dotnetExam2.DTOs
{
    public record CreateMovieDto(
        string Title, 
        string Genre, 
        DateTimeOffset ReleaseDate, 
        double Rating);
}

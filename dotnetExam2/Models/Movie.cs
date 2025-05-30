namespace dotnetExam2.Models
{
    public sealed class Movie : EntityBase
    {
        public string Title { get; private set; }
        public string Genre { get; private set; }
        public DateTimeOffset ReleaseDate { get; private set; }
        public double Rating { get; set; }

        // Private constructor for ORM frameworks
        private Movie()
        {
            Title = string.Empty;
            Genre = string.Empty;
        }
        private Movie(string title, string genre, DateTimeOffset releaseDate, double rating)
        {
            Title = title;
            Genre = genre;
            ReleaseDate = releaseDate;
            Rating = rating;
        }

        public static Movie Create(string title, string genre, DateTimeOffset releaseDate, double rating)
        {
            return new Movie(title, genre, releaseDate, rating);
        }

        public void Update(string title, string genre, DateTimeOffset releaseDate, double rating)
        {

            Title = title;
            Genre = genre;
            ReleaseDate = releaseDate;
            Rating = rating;

            UpdateLastModified();
        }
    }
}

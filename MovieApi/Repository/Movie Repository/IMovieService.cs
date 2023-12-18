using MovieApi.Models;

namespace MovieApi.Repository.Movie_Repository
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMovie(byte genreId=0);
        Task <Movie> GetMovieById (int id);
        Task <Movie> AddMovie (Movie movie);
        Movie UpdateMovie (Movie movie);
        Movie DeleteMovie(Movie movie);
    }
}

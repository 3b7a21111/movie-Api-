using Microsoft.EntityFrameworkCore;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Repository.Movie_Repository
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDBcontext context;

        public MovieService(ApplicationDBcontext context)
        {
            this.context = context;
        }
        public async Task<Movie> AddMovie(Movie movie)
        {
            await context.AddAsync(movie);
            context.SaveChanges();
            return movie;
        }

        public  Movie DeleteMovie(Movie movie)
        {
            context.Remove(movie);
            context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAllMovie(byte genreId = 0)
        {
            return await context.Movies
                .Where(m=>m.GenreId == genreId ||genreId==0)
                .OrderByDescending(x => x.Rate)
                .Include(x => x.Genre)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var movie= await context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            return movie!; 
        }
        public Movie UpdateMovie(Movie movie)
        {
            context.Update(movie);
            context.SaveChanges();
            return movie;
        }
    }
}

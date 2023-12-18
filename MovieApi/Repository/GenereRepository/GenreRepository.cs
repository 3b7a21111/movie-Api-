using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Repository.GenereRepository
{
    public class GenreRepository:IGenreRepository
    {
        private readonly ApplicationDBcontext _context;

        public GenreRepository(ApplicationDBcontext context)
        {
            _context = context;
        }

        public Task<bool> IsValidGenre(byte id)
        {
            return _context.Genres.AnyAsync(x => x.Id == id);
        }
        public async Task<Genre> Add(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();

            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();

            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre> GetById(byte id)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
        }


        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();

            return genre;
        }
    }
}

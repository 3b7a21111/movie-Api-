using MovieApi.Models;
using System.Collections.Generic;

namespace MovieApi.Repository.GenereRepository
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(byte id);
        Task<Genre> Add(Genre genre);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);
        Task<bool> IsValidGenre(byte id);

    }
}

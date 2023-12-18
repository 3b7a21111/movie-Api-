using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.DTOs;
using MovieApi.Models;
using MovieApi.Repository.GenereRepository;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await genreRepository.GetAll();

            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };

            await genreRepository.Add(genre);

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await genreRepository.GetById(id);

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genre.Name = dto.Name;

            genreRepository.Update(genre);

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await genreRepository.GetById(id);

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genreRepository.Delete(genre);

            return Ok(genre);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.DTOs;
using MovieApi.Models;
using MovieApi.Repository.GenereRepository;
using MovieApi.Repository.Movie_Repository;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly List<string> _AllowedExtension = new List<string> { ".jpg", ".png", ".jpeg" };
        private readonly long _maxallowedpostersize = 1048576;
        private readonly IMovieService movieService;
        private readonly IGenreRepository genreRepository;
        private readonly IMapper mapper;

        public MoviesController(IMovieService movieService, IGenreRepository genreRepository, IMapper mapper)
        {
            this.movieService = movieService;
            this.genreRepository = genreRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await movieService.GetAllMovie();
            //auto mapper
            var data = mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();
            var dto = mapper.Map<MovieDetailsDto>(movie);
            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await movieService.GetAllMovie(genreId);
            //auto mapper
            var data = mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }
            [HttpPost]
            public async Task<IActionResult> Add([FromForm] MovieDto dto)
            {
                if (dto.Poster == null)
                    return BadRequest("Poster is required");
                //check extension
                if (!_AllowedExtension.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png or .jpg extension images are allowed");
                //check size    
                if (dto.Poster.Length > _maxallowedpostersize)
                    return BadRequest("Max allowed size for poster is 1MB");
                //check GenerId is exist
                var isvalidGenre = await genreRepository.IsValidGenre(dto.GenreId);
                if (!isvalidGenre)
                    return BadRequest("Invalid Genre ID!");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                var movie = mapper.Map<Movie>(dto);
                movie.Poster = dataStream.ToArray();
                await movieService.AddMovie(movie);

                return Ok(movie);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
            {
                var movie = await movieService.GetMovieById(id);

                if (movie == null)
                    return BadRequest($"No movie with this id :{id}");

                var isvalidGenre = await genreRepository.IsValidGenre(dto.GenreId);
                if (!isvalidGenre)
                    return BadRequest("Invalid Genre ID!");
                if (dto.Poster != null)
                {
                    if (!_AllowedExtension.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                        return BadRequest("Only .png or .jpg extension images are allowed");

                    if (dto.Poster.Length > _maxallowedpostersize)
                        return BadRequest("Max allowed size for poster is 1MB");

                    using var dataStream = new MemoryStream();
                    await dto.Poster.CopyToAsync(dataStream);

                    movie.Poster = dataStream.ToArray();
                }
                movie.Title = dto.Title;
                movie.Year = dto.Year;
                movie.Rate = dto.Rate;
                movie.Storeline = dto.Storeline;
                movie.GenreId = dto.GenreId;

                movieService.UpdateMovie(movie);
                return Ok(movie);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var movie = await movieService.GetMovieById(id);
                if (movie == null)
                    return NotFound($"No movie with this id :{id}");
                movieService.DeleteMovie(movie);
                return Ok(movie);
            }

        }
    }

using AutoMapper;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<MovieDto, Movie>()
                .ForMember(src=>src.Poster,opt=>opt.Ignore());
        }
    }
}

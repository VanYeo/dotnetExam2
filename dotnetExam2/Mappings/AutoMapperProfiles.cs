using AutoMapper;
using dotnetExam2.DTOs;
using dotnetExam2.Models;

namespace dotnetExam2.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<MovieDto, Movie>().ReverseMap();
            CreateMap<CreateMovieDto, Movie>().ReverseMap();
            CreateMap<UpdateMovieDto, Movie>().ReverseMap();

        }
    }
}

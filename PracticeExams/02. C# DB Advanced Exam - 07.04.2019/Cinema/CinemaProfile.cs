using System;
using System.Globalization;
using AutoMapper;
using Cinema.Data.Models;
using Cinema.DataProcessor.ImportDto;

namespace Cinema
{
    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            CreateMap<ImportMovieDto, Movie>()
                .ForMember(movie => movie.Duration,
                    expression => expression
                        .MapFrom(k => TimeSpan.ParseExact(
                            k.Duration, @"hh\:mm\:ss", CultureInfo.InvariantCulture)));

            CreateMap<ImportProjectionDto, Projection>()
                .ForMember(p => p.DateTime,
                    y => y.MapFrom(k => DateTime.ParseExact(k.DateTime, @"yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)))
                .ForMember(p => p.HallId,
                    expression => expression.MapFrom(k => k.HallId))
                .ForMember(p => p.MovieId,
                    expression => expression.MapFrom(k => k.MovieId));
            CreateMap<ImportCustomerTicketsDto, Customer>();

        }
    }
}

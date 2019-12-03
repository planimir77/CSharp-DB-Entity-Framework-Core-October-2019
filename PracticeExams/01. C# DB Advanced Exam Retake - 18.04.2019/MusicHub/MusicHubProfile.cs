using System;
using System.Globalization;
using MusicHub.Data.Models;
using MusicHub.DataProcessor.ImportDtos;

namespace MusicHub
{
    using AutoMapper;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            CreateMap<ImportWritersDto, Writer>();

            CreateMap<ImportProducerAlbumsDto, Producer>();

            CreateMap<ImportAlbumDto, Album>()
                .ForMember(album => album.ReleaseDate,
                    expression => expression
                        .MapFrom(dto => DateTime.ParseExact(
                            dto.ReleaseDate, @"dd/MM/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<ImportSongDto, Song>()
                .ForMember(t => t.Duration, y => y.MapFrom(k => TimeSpan.ParseExact(k.Duration, @"hh\:mm\:ss", CultureInfo.InvariantCulture)))
                .ForMember(t => t.CreatedOn, y => y.MapFrom(k => DateTime.ParseExact(k.CreatedOn, @"dd/MM/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<ImportSongPerformerDto, Performer>();
        }
    }
}

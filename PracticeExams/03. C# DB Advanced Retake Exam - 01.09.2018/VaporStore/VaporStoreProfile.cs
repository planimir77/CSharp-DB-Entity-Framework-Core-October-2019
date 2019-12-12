using System;
using System.Globalization;
using VaporStore.Data.Models;
using VaporStore.DataProcessor.ImportDtos;

namespace VaporStore
{
	using AutoMapper;

	public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
        {
            //CreateMap<ImportGameDto, Game>()
            //    .ForMember(t => t.ReleaseDate, y => y
            //            .MapFrom(k => DateTime
            //                .ParseExact(k.ReleaseDate, @"yyyy-MM-dd", CultureInfo.InvariantCulture)));
            //CreateMap<ImportUsersDto, User>();

            //CreateMap<ImportPurchasesDto, Purchase>();
        }
	}
}
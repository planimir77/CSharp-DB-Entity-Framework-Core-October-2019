using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using MusicHub.Data.Models;
using MusicHub.Data.Models.Enums;
using MusicHub.DataProcessor.ImportDtos;
using Newtonsoft.Json;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace MusicHub.DataProcessor
{
    using System;

    using Data;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var itemsDtos = JsonConvert.DeserializeObject<ImportWritersDto[]>(jsonString);

            var sb = new StringBuilder();

            var items = new List<Writer>();

            foreach (var itemDto in itemsDtos)
            {
                Writer writer = Mapper.Map<Writer>(itemDto);

                if (!IsValid(writer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                items.Add(writer);

                sb.AppendLine(String.Format(SuccessfullyImportedWriter, writer.Name));

            }
            context.Writers.AddRange(items);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producersDtos = JsonConvert.DeserializeObject<ImportProducerAlbumsDto[]>(jsonString);

            var sb = new StringBuilder();

            var producers = new List<Producer>();

            foreach (var producerDto in producersDtos)
            {
                if (!IsValid(producerDto) || !producerDto.Albums.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Producer producer = Mapper.Map<Producer>(producerDto);

                producers.Add(producer);

                if (producer.PhoneNumber == null)
                {
                    sb.AppendLine(String.Format(SuccessfullyImportedProducerWithNoPhone,
                        producer.Name, producer.Albums.Count));
                }
                else
                {
                    sb.AppendLine(String.Format(SuccessfullyImportedProducerWithPhone, 
                        producer.Name, producer.PhoneNumber, producer.Albums.Count));
                }

            }
            context.Producers.AddRange(producers);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportSongDto>),
                new XmlRootAttribute("Songs"));

            var reader = new StringReader(xmlString);

            var songsDto = (List<ImportSongDto>)serializer.Deserialize(reader);

            var sb = new StringBuilder();
            var songs = new List<Song>();

            
            foreach (var songDto in songsDto)
            {
                bool isValidEnum = Enum.IsDefined(typeof(Genre),songDto.Genre);

                Writer writer = context.Writers.Find(songDto.WriterId);

                Album album = context.Albums.Find(songDto.AlbumId);

                var songExist = songs.Any(song1 => song1.Name == songDto.Name);

                if (writer == null || (album == null && songDto.AlbumId != null) || !isValidEnum || songExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Song song = Mapper.Map<Song>(songDto);
                
                if (!IsValid(song))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                songs.Add(song);

                sb.AppendLine(String.Format(SuccessfullyImportedSong,
                    song.Name,
                    song.Genre.ToString(), 
                    song.Duration.ToString(@"hh\:mm\:ss")));
            }

            context.Songs.AddRange(songs);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<ImportSongPerformerDto>),
                new XmlRootAttribute("Performers"));

            var reader = new StringReader(xmlString);

            var songPerformersDto = (List<ImportSongPerformerDto>)serializer.Deserialize(reader);

            var performers = new List<Performer>();

            var songPerformers= new List<SongPerformer>();

            var sb = new StringBuilder();


            foreach (var songPerformerDto in songPerformersDto)
            {
                
                var performer = Mapper.Map<Performer>(songPerformerDto);

                var isAllSongsValid = songPerformerDto.PerformersSongs
                    .All(x => context.Songs.Any(song => song.Id == x.Id));

                if (!IsValid(songPerformerDto) || !isAllSongsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var songDtoId in songPerformerDto.PerformersSongs)
                {
                    var entity = songPerformers
                        .Count(songPerformer => songPerformer.Performer == performer &&
                                          songPerformer.SongId == songDtoId.Id);

                    if (context.Songs.Find(songDtoId.Id) != null && entity == 0)
                    {

                        var partCar = new SongPerformer
                        {
                            Performer = performer,
                            SongId = songDtoId.Id
                        };

                        songPerformers.Add(partCar);
                    }
                }
                performers.Add(performer);
                
                sb.AppendLine(String.Format(SuccessfullyImportedPerformer,performer.FirstName,songPerformerDto.PerformersSongs.Count));
            }

            context.Performers.AddRange(performers);

            context.SongPerformers.AddRange(songPerformers);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            ValidationContext validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(
                entity, validationContext, validationResult, true);
        }
    }
}
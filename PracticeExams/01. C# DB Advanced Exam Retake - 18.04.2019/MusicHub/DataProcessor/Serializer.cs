using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using MusicHub.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using Remotion.Linq.Clauses;
using Formatting = Newtonsoft.Json.Formatting;

namespace MusicHub.DataProcessor
{
    using System;

    using Data;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var songsDto = context.Producers
                .Find(producerId)
                .Albums
                .Select(album => new ExportAlbumsInfoDto
                {
                    AlbumName = album.Name,
                    ReleaseDate = album.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = album.Producer.Name,
                    Songs = album.Songs.Select(song => new ExportSongDto
                    {
                        SongName = song.Name,
                        Price = song.Price.ToString("F2"),
                        Writer = song.Writer.Name
                    })
                        .OrderByDescending(dto => dto.SongName)
                        .ThenBy(dto => dto.Writer)
                    .ToList(),
                    AlbumPrice = $"{album.Price:F2}"
                })
                .OrderByDescending(dto => Convert.ToDecimal(dto.AlbumPrice))
                .ToList();

            var json = JsonConvert.SerializeObject(songsDto, Formatting.Indented);
           
            return json;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsXml = context
                .Songs
                .Where(song => song.Duration.TotalSeconds > duration)
                .Select(song => new ExportSongsAboveDurationDto
                {
                    SongName = song.Name,
                    Writer = song.Writer.Name,
                    Performer = $"{song.SongPerformers.FirstOrDefault(performer => performer.SongId == song.Id).Performer.FirstName} {song.SongPerformers.FirstOrDefault(performer => performer.SongId == song.Id).Performer.LastName}",
                    AlbumProducer = song.Album.Producer.Name,
                    Duration = song.Duration.ToString("c",CultureInfo.InvariantCulture)
                })
                .OrderBy(dto => dto.SongName)
                .ThenBy(dto => dto.Writer)
                .ThenBy(dto => dto.Performer)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportSongsAboveDurationDto[]),
                new XmlRootAttribute("Songs"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, songsXml, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}

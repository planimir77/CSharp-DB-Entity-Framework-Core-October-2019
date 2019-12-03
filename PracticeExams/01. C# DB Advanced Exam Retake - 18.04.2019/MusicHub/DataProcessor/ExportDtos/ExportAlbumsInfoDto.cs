using System;
using System.Collections.Generic;
using System.Text;
using MusicHub.Data.Models;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class ExportAlbumsInfoDto
    {
        public string AlbumName { get; set; }
        public string ReleaseDate { get; set; }
        public string ProducerName { get; set; }
        public List<ExportSongDto> Songs { get; set; }
        public string AlbumPrice { get; set; }
    }
    public class ExportSongDto
    {
        public string SongName { get; set; }
        public string Price { get; set; }
        public string Writer { get; set; }
    }
}

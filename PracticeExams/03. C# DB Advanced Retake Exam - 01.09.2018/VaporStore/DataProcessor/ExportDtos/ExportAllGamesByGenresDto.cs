using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ExportDtos
{
    public class ExportAllGamesByGenresDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [JsonProperty("Games")]
        public List<ExportGameDto> GamesDto { get; set; }

        [JsonProperty("TotalPlayers")]
        public int TotalPlayers { get; set; }
    }
}

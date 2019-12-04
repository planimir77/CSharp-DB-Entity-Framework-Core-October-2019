using System;
using System.Collections.Generic;
using System.Text;
using Cinema.Data.Models;
using Newtonsoft.Json;

namespace Cinema.DataProcessor.ExportDto
{
    public class ExportTopMoviesDto
    {
        [JsonProperty("MovieName")]
        public string MovieName { get; set; }

        [JsonProperty("Rating")]
        public string Rating { get; set; }

        [JsonProperty("TotalIncomes")]
        public string TotalIncomes { get; set; }

        [JsonProperty("Customers")]
        public List<CustomerDto> Customers { get; set; }

    }
}

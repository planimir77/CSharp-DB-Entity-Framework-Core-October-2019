using System;
using System.Collections.Generic;
using System.Text;
using BookShop.Data.Models;
using Newtonsoft.Json;

namespace BookShop.DataProcessor.ExportDto
{
    class ExportMostCraziestAuthorDto
    {
        [JsonProperty("AuthorName")]
        public string AuthorName { get; set; }

        [JsonProperty("Books")]
        public List<BookExportDto> Books { get; set; }
    }
}

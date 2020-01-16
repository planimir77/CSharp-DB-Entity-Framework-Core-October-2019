using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BookShop.DataProcessor.ExportDto
{
    public class BookExportDto
    {
        
        [JsonProperty("BookName")]
        public string BookName { get; set; }

        [JsonProperty("BookPrice")]
        public string BookPrice { get; set; }
    }
}

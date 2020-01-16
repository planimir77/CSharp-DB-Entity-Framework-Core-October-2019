using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace BookShop.DataProcessor.ImportDto
{
    public class BookDtos
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }
    }
}

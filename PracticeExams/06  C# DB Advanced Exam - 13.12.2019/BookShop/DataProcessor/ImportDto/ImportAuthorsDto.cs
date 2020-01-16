using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BookShop.Data.Models;
using Newtonsoft.Json;

namespace BookShop.DataProcessor.ImportDto
{
    public class ImportAuthorsDto
    {
        [Required] 
        [MinLength(3), MaxLength(30)]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [Required] 
        [MinLength(3), MaxLength(30)]
        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Books")]
        public List<BookDtos> Books { get; set; }
    }
}

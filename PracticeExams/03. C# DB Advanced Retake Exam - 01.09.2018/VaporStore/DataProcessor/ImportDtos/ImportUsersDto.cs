using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.ImportDtos
{
    public partial class ImportUsersDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        [JsonProperty("Username")]
        public string Username { get; set; }

        [Required]
        [RegularExpression("^[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+$")]
        [JsonProperty("FullName")]
        public string FullName { get; set; }

        [Required]
        [JsonProperty("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(3, 103)]
        [JsonProperty("Age")]
        public int Age { get; set; }

        [JsonProperty("Cards")]
        public List<CardDto> CardsDto { get; set; }
    }
}

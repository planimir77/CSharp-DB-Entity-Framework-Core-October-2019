using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TeisterMask.DataProcessor.ImportDto
{
    public class ImportEmployeeDto
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        [RegularExpression("^[0-9A-Za-z]+$")]
        [JsonProperty("Username")]
        public string Username { get; set; }

        [Required]
        [JsonProperty("Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("Tasks")]
        public List<int> TasksId { get; set; }
    }
}

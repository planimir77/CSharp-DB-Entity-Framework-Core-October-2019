using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.ImportDtos
{
    public partial class CardDto
    {
        [Required]
        [JsonProperty("Number")]
        [RegularExpression("^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}")]
        [JsonProperty("CVC")]
        public string Cvc { get; set; }

        [Required] 
        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}
